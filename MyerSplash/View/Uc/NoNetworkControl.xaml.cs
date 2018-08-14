using MyerSplashCustomControl;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyerSplash.View.Uc
{
    public sealed partial class NoNetworkControl : UserControl
    {
        public NoNetworkControl()
        {
            this.InitializeComponent();
        }

        private async void DiagnoseButton_Click(object sender, RoutedEventArgs e)
        {
            var uc = new NetworkDiagnosisDialog();
            await PopupService.Instance.ShowAsync(uc);
        }
    }
}