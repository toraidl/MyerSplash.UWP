using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace MyerSplashCustomControl.Control
{
    public enum BackdropType
    {
        HostAcrylic,
        Acrylic
    }

    public class MaterialCompatGrid : Grid
    {
        public BackdropType BackdropType
        {
            get { return (BackdropType)GetValue(BackdropTypeProperty); }
            set { SetValue(BackdropTypeProperty, value); }
        }

        public static readonly DependencyProperty BackdropTypeProperty =
            DependencyProperty.Register("BackdropType",
                typeof(BackdropType), typeof(MaterialCompatGrid),
                new PropertyMetadata(BackdropType.Acrylic, (s, e) =>
                {
                    var control = s as MaterialCompatGrid;
                    control.UpdateBackground();
                }));

        public bool LightEnabled
        {
            get { return (bool)GetValue(LightEnabledProperty); }
            set { SetValue(LightEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LightEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LightEnabledProperty =
            DependencyProperty.Register("LightEnabled", typeof(bool),
                typeof(MaterialCompatGrid), new PropertyMetadata(false, (s, e) =>
                {
                    var control = s as MaterialCompatGrid;
                    if ((bool)e.NewValue)
                    {
                        control.UpdateLight();
                    }
                }));

        private XamlLight _hoverLight;

        public MaterialCompatGrid()
        {
            UpdateBackground();
        }

        private void UpdateLight()
        {
            if (_hoverLight == null)
            {
                _hoverLight = new HoverLight();
            }
            if (!Lights.Contains(_hoverLight))
            {
                Lights.Add(_hoverLight);
                Lights.Add(new AmbLight());
            }
        }

        private void UpdateBackground()
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.Xaml.Media.XamlCompositionBrushBase"))
            {
                var brush = new AcrylicBrush
                {
                    BackgroundSource = BackdropType == BackdropType.Acrylic ? AcrylicBackgroundSource.Backdrop : AcrylicBackgroundSource.HostBackdrop,
                    TintColor = Colors.Black,
                    FallbackColor = Colors.Black,
                    TintOpacity = 0.8
                };

                Background = brush;
            }
            else
            {
                Brush.AcrylicBrushBase brush;
                switch (BackdropType)
                {
                    case BackdropType.Acrylic:
                        brush = new Brush.AcrylicBrush();
                        break;

                    default:
                        brush = new Brush.AcrylicHostBrush();
                        break;
                }
                Background = brush;
            }
        }
    }
}