using MyerSplash.Common;
using MyerSplash.Common.Composition;
using MyerSplashCustomControl.Adapter;
using System;
using System.Numerics;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyerSplash.Adapter
{
    public class AnimatedAdapter : IListViewAdapter
    {
        protected ListViewBase _listView;
        protected Compositor _compositor;
        protected ImplicitAnimationCollection _elementImplicitAnimation;

        public virtual void OnAttachToListView(ListViewBase listView)
        {
            _listView = listView;
            if (listView != null && _compositor == null)
            {
                _compositor = _listView.GetVisual().Compositor;
                _elementImplicitAnimation = ImplicitAnimationFactory.CreateListOffsetAnimationCollection(_compositor);
            }
        }

        public virtual void OnChoosingItemContainer(ListViewBase sender, ChoosingItemContainerEventArgs args)
        {

        }

        public virtual void OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            int index = args.ItemIndex;

            if (!args.InRecycleQueue)
            {
                args.ItemContainer.Loaded -= ItemContainer_Loaded;
                args.ItemContainer.Loaded += ItemContainer_Loaded;
            }

            var elementVisual = args.ItemContainer.GetVisual();
            if (args.InRecycleQueue)
            {
                elementVisual.ImplicitAnimations = null;
            }
            else
            {
                elementVisual.ImplicitAnimations = _elementImplicitAnimation;
            }
        }

        private void ItemContainer_Loaded(object sender, RoutedEventArgs e)
        {
            if (_listView == null || _compositor == null) return;

            var itemsPanel = (ItemsWrapGrid)_listView.ItemsPanelRoot;
            var itemContainer = (GridViewItem)sender;
            var itemIndex = _listView.IndexFromContainer(itemContainer);

            // Don't animate if we're not in the visible viewport
            if (itemIndex >= itemsPanel.FirstVisibleIndex && itemIndex <= itemsPanel.LastVisibleIndex)
            {
                var itemVisual = itemContainer.GetVisual();
                var delayIndex = itemIndex - itemsPanel.FirstVisibleIndex;

                itemVisual.Opacity = 0f;
                itemVisual.SetTranslation(new Vector3(50, 0, 0));

                // Create KeyFrameAnimations
                var offsetAnimation = _compositor.CreateScalarKeyFrameAnimation();
                offsetAnimation.InsertKeyFrame(1f, 0f);
                offsetAnimation.Duration = TimeSpan.FromMilliseconds(700);
                offsetAnimation.DelayTime = TimeSpan.FromMilliseconds((delayIndex * 30));

                var fadeAnimation = _compositor.CreateScalarKeyFrameAnimation();
                fadeAnimation.InsertKeyFrame(1f, 1f);
                fadeAnimation.Duration = TimeSpan.FromMilliseconds(700);
                fadeAnimation.DelayTime = TimeSpan.FromMilliseconds(delayIndex * 30);

                // Start animations
                itemVisual.StartAnimation(itemVisual.GetTranslationXPropertyName(), offsetAnimation);
                itemVisual.StartAnimation("Opacity", fadeAnimation);
            }
            itemContainer.Loaded -= ItemContainer_Loaded;
        }
    }
}
