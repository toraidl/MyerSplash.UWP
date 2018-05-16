using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace MyerSplashCustomControl
{
    [ContentProperty(Name = "Items")]
    public class TopNavigationControl : Control
    {
        private StackPanel _rootPanel;
        private Border _navigationBorder;
        private Visual _borderVisual;

        public ICollection<Object> Items
        {
            get;
        }

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int),
                typeof(TopNavigationControl), new PropertyMetadata(0, (s, r) =>
                {
                    TopNavigationControl target = s as TopNavigationControl;
                    target.UpdateSelectedSlider();
                }));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate),
                typeof(TopNavigationControl), new PropertyMetadata(null, (s, r) =>
                {
                    TopNavigationControl target = s as TopNavigationControl;
                    target.UpdateViews();
                }));

        protected virtual void OnItemTemplateChanged(DataTemplate oldValue, DataTemplate newValue)
        {
            UpdateViews();
        }

        public TopNavigationControl()
        {
            this.DefaultStyleKey = typeof(TopNavigationControl);
            var items = new ObservableCollection<object>();
            items.CollectionChanged += Items_CollectionChanged;
            Items = items;
        }

        protected virtual DependencyObject GetContainerForItemOverride()
        {
            return new ContentPresenter();
        }

        protected virtual bool IsItemItsOwnContainerOverride(System.Object item)
        {
            return item is UIElement;
        }

        protected virtual void PrepareContainerForItemOverride(DependencyObject container, System.Object item)
        {
            if (container is ContentControl)
            {
                ContentControl control = container as ContentControl;
                control.Content = item;
                control.ContentTemplate = ItemTemplate;
            }
            else if (container is ContentPresenter)
            {
                ContentPresenter presenter = container as ContentPresenter;
                presenter.Content = item;
                presenter.ContentTemplate = ItemTemplate;
            }
        }

        private void UpdateSelectedSlider()
        {

        }

        private void UpdateViews()
        {
            if (_rootPanel == null) return;

            _rootPanel.Children.Clear();

            foreach (var item in Items)
            {
                DependencyObject container;
                if (IsItemItsOwnContainerOverride(item))
                {
                    container = item as DependencyObject;
                }
                else
                {
                    container = GetContainerForItemOverride();
                    PrepareContainerForItemOverride(container, item);
                }

                if (container is UIElement)
                {
                    _rootPanel.Children.Add(container as UIElement);
                }
            }
        }

        private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateViews();
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _rootPanel = GetTemplateChild("RootPanel") as StackPanel;
            _rootPanel.SizeChanged += _rootPanel_SizeChanged;
            _navigationBorder = GetTemplateChild("NavigationBorder") as Border;
            _borderVisual = ElementCompositionPreview.GetElementVisual(_navigationBorder);
            UpdateViews();
        }

        private void _rootPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (SelectedIndex < 0 || SelectedIndex >= _rootPanel.Children.Count) return;

            UIElement element = _rootPanel.Children[SelectedIndex];
            if (element is ContentPresenter)
            {
                var child = VisualTreeHelper.GetChild(element, 0) as FrameworkElement;
                var transform = element.TransformToVisual(_rootPanel);
                var point = transform.TransformPoint(new Point(0, child.ActualHeight));
                _borderVisual.Offset = new Vector3((float)point.X, 0, 0);
            }
        }
    }
}
