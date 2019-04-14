using Microsoft.Toolkit.Uwp.UI.Controls;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace MyerSplashCustomControl
{
    public enum LayoutStretch
    {
        Stretch,
        Center,
        Bottom
    }

    public class ContentPopupEx : ContentControl
    {
        private readonly TaskCompletionSource<int> _tcs;

        private Grid _rootGrid;
        private Grid _contentGrid;
        private FrameworkElement _rootFramework;
        private readonly bool _solidBackground;
        private Border _maskBorder;

        private readonly Popup _currentPopup;
        private readonly LayoutStretch _layoutStretch;

        public bool PlayPopupAnim = true;

        private Storyboard _inStory;
        private Storyboard _outStory;

        public Page CurrentPage => ((Window.Current.Content as Frame).Content) as Page;

        private bool _isOpen;

        public bool AllowTapMaskToHide { get; set; } = true;

        private ContentPopupEx()
        {
            DefaultStyleKey = typeof(ContentPopupEx);

            _tcs = new TaskCompletionSource<int>();

            if (_currentPopup == null)
            {
                _currentPopup = new Popup {VerticalAlignment = VerticalAlignment.Stretch};
                this.Height = (Window.Current.Content as Frame).Height;
                this.Width = (Window.Current.Content as Frame).Width;
                _currentPopup.Child = this;
                _currentPopup.IsOpen = true;
            }

            CurrentPage.SizeChanged += Page_SizeChanged;
        }

        public ContentPopupEx(FrameworkElement element, bool solidBackground = true,
            LayoutStretch layout = LayoutStretch.Center) : this()
        {
            _rootFramework = element;
            _layoutStretch = layout;
            _solidBackground = solidBackground;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _rootGrid = GetTemplateChild("RootGrid") as Grid;
            _contentGrid = GetTemplateChild("ContentGrid") as Grid;
            switch (_layoutStretch)
            {
                case LayoutStretch.Stretch:
                    _contentGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
                    _contentGrid.VerticalAlignment = VerticalAlignment.Stretch;
                    break;
                case LayoutStretch.Bottom:
                    _contentGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
                    _contentGrid.VerticalAlignment = VerticalAlignment.Bottom;
                    break;
            }
            _contentGrid.Children.Add(_rootFramework);

            if (!_solidBackground)
            {
                var shadowControl = GetTemplateChild("RootPanel") as DropShadowPanel;
                shadowControl.ShadowOpacity = 0f;
            }

            _inStory = _rootGrid.Resources["InStory"] as Storyboard;
            _outStory = _rootGrid.Resources["OutStory"] as Storyboard;
            _maskBorder = GetTemplateChild("MaskBorder") as Border;
            _outStory.Completed += ((sender, e) =>
            {
                _currentPopup.IsOpen = false;
            });
            _maskBorder.Tapped += ((_, et) =>
            {
                if (!AllowTapMaskToHide) return;
                if (!_isOpen)
                {
                    return;
                }
                PopupService.Instance.TryHide();
            });
            _tcs.TrySetResult(0);
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateCurrentLayout();
        }

        private void UpdateCurrentLayout()
        {
            _rootGrid.Width = this.Width = Window.Current.Bounds.Width;
            _rootGrid.Height = this.Height = Window.Current.Bounds.Height;
        }

        public async Task ShowAsync()
        {
            await _tcs.Task;
            UpdateCurrentLayout();
            _maskBorder.Visibility = Visibility.Visible;
            _isOpen = true;
            if (PlayPopupAnim)
            {
                _inStory.Begin();
            }
        }

        public void Hide()
        {
            CurrentPage.SizeChanged -= Page_SizeChanged;
            _rootGrid.Children.Remove(_rootFramework);
            _rootFramework = null;
            _isOpen = false;
            _outStory.Begin();
            _maskBorder.Visibility = Visibility.Collapsed;
        }
    }
}