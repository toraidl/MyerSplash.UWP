using MyerSplash.Common;
using MyerSplash.Model;
using MyerSplash.ViewModel;
using MyerSplashShared.Utils;
using System;
using System.Numerics;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;

namespace MyerSplash.View.Page
{
    public sealed partial class MainPage : CustomizedTitleBarPage
    {
        private const float TITLE_GRID_HEIGHT = 70;

        private MainViewModel MainVM { get; set; }

        private Compositor _compositor;
        private Visual _refreshBtnVisual;

        private double _lastVerticalOffset;
        private bool _isHideTitleGrid;

        private ImageItem _clickedImg;
        private FrameworkElement _clickedContainer;

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(MainViewModel),
                new PropertyMetadata(false, OnLoadingPropertyChanged));

        public static void OnLoadingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var page = d as MainPage;
            if (!(bool)e.NewValue)
            {
                page.HideLoading();
            }
            else page.ShowLoading();
        }

        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = MainVM = new MainViewModel();
            InitComposition();
            InitBinding();
        }

        private void InitBinding()
        {
            var b = new Binding()
            {
                Source = MainVM,
                Path = new PropertyPath("IsRefreshing"),
                Mode = BindingMode.TwoWay,
            };
            this.SetBinding(IsLoadingProperty, b);
        }

        private void InitComposition()
        {
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            _refreshBtnVisual = RefreshBtn.GetVisual();
        }

        #region Loading animation

        private void ShowLoading()
        {
            ListControl.Refreshing = true;
        }

        private void HideLoading()
        {
            ListControl.Refreshing = false;
        }

        #endregion Loading animation

        private void StackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ListControl.ScrollToTop();
        }

        private void ListControl_OnClickItemStarted(ImageItem img, FrameworkElement container)
        {
            _clickedContainer = container;
            _clickedImg = img;

            ToggleDetailControlAnimation();
        }

        private void ToggleDetailControlAnimation()
        {
            DetailControl.CurrentImage = _clickedImg;
            DetailControl.Show(_clickedContainer);

            NavigationService.AddOperation(() =>
            {
                DetailControl.Hide();
                return true;
            });
        }

        #region Scrolling

        private void ToggleRefreshBtnAnimation(bool show)
        {
            var offsetAnimation = _compositor.CreateScalarKeyFrameAnimation();
            offsetAnimation.InsertKeyFrame(1f, show ? 1f : 0);
            offsetAnimation.Duration = TimeSpan.FromMilliseconds(500);

            _refreshBtnVisual.CenterPoint = new Vector3((float)RefreshBtn.ActualWidth / 2f, (float)RefreshBtn.ActualHeight / 2f, 0f);
            _refreshBtnVisual.StartAnimation("Scale.X", offsetAnimation);
            _refreshBtnVisual.StartAnimation("Scale.Y", offsetAnimation);
        }

        private void ListControl_OnScrollViewerViewChanged(ScrollViewer scrollViewer)
        {
            if (DeviceUtil.IsXbox) return;

            if ((scrollViewer.VerticalOffset - _lastVerticalOffset) > 5 && !_isHideTitleGrid)
            {
                _isHideTitleGrid = true;
                ToggleRefreshBtnAnimation(false);
            }
            else if (scrollViewer.VerticalOffset < _lastVerticalOffset && _isHideTitleGrid)
            {
                _isHideTitleGrid = false;
                ToggleRefreshBtnAnimation(true);
            }
            _lastVerticalOffset = scrollViewer.VerticalOffset;
        }
        #endregion Scrolling

        protected override void SetUpTitleBar()
        {            
            TitleBarHelper.SetUpLightTitleBar();
        }

        private void OnShownChanged(object sender, ShownArgs e)
        {
            if (!e.Shown)
            {
                Window.Current.SetTitleBar(DummyTitleBar);
            }
        }

        private void MoreBtn_Click(object sender, RoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }
    }
}