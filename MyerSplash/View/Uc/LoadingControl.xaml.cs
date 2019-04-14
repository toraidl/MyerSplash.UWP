using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace MyerSplash.View.Uc
{
    public sealed partial class LoadingControl : UserControl
    {
        public LoadingControl()
        {
            this.InitializeComponent();

            if (!DesignMode.DesignModeEnabled)
            {
                this.SizeChanged += LoadingControl_SizeChanged;
                Start();
            }
        }

        private void LoadingControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Clip = new RectangleGeometry()
            {
                Rect = new Rect(0, 0, e.NewSize.Width, e.NewSize.Height),
            };
        }

        public void Start()
        {
            StartStory.Begin();
        }

        public void Stop()
        {
            StartStory.Stop();
        }
    }
}