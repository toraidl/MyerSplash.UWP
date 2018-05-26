using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using MyerSplash.Common;
using MyerSplash.Common.Composition;
using MyerSplash.Model;
using MyerSplash.ViewModel;
using System;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyerSplash.View.Uc
{
    public sealed partial class ManageDownloadControl : NavigableUserControl
    {
        public DownloadsViewModel DownloadsVM
        {
            get
            {
                return SimpleIoc.Default.GetInstance<DownloadsViewModel>();
            }
        }

        public RelayCommand CloseCommand
        {
            get { return (RelayCommand)GetValue(CloseCommandProperty); }
            set { SetValue(CloseCommandProperty, value); }
        }

        public static readonly DependencyProperty CloseCommandProperty =
            DependencyProperty.Register("CloseCommand", typeof(RelayCommand), typeof(ManageDownloadControl),
                new PropertyMetadata(null));

        private Compositor _compositor;
        private ImplicitAnimationCollection _elementImplicitAnimation;

        public ManageDownloadControl()
        {
            this.InitializeComponent();

            _compositor = this.GetVisual().Compositor;
            _elementImplicitAnimation = ImplicitAnimationFactory.CreateListOffsetAnimationCollection(_compositor);
        }

        public void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            CloseCommand?.Execute(null);
        }

        public override void OnPresented()
        {
            base.OnPresented();
            Window.Current.SetTitleBar(DummyTitleBar);
        }

        private async void ImageGridView_ChoosingItemContainer(ListViewBase sender, ChoosingItemContainerEventArgs args)
        {
            if (!(args.Item is DownloadItem item)) return;
            var cacheBitmap = item.ImageItem.ListImageBitmap;
            var bitmap = cacheBitmap.Bitmap;
            if (bitmap == null)
            {
                await cacheBitmap.LoadBitmapAsync();
            }
        }

        private void ImageGridView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var elementVisual = args.ItemContainer.GetVisual();
            if (args.InRecycleQueue)
            {
                elementVisual.ImplicitAnimations = null;
            }
            else
            {
                //Add implicit animation to each visual 
                elementVisual.ImplicitAnimations = _elementImplicitAnimation;
            }
        }
    }
}