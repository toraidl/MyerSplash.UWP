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

    public class AcrylicCompatGrid : Grid
    {
        public BackdropType BackdropType
        {
            get { return (BackdropType)GetValue(BackdropTypeProperty); }
            set { SetValue(BackdropTypeProperty, value); }
        }

        public static readonly DependencyProperty BackdropTypeProperty =
            DependencyProperty.Register("BackdropType",
                typeof(BackdropType), typeof(AcrylicCompatGrid),
                new PropertyMetadata(BackdropType.Acrylic, (s, e) =>
                {
                    var control = s as AcrylicCompatGrid;
                    control.UpdateBackground();
                }));

        public AcrylicCompatGrid()
        {
            UpdateBackground();
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