using MyerSplashCustomControl;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyerSplash.View.Uc
{
    public sealed partial class TipsControl : UserControl
    {
        public TipsControl()
        {
            this.InitializeComponent();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            PopupService.Instance.TryHide();
        }

        private async void DiagnoseButton_Click(object sender, RoutedEventArgs e)
        {
            var uc = new NetworkDiagnosisDialog();
            await PopupService.Instance.ShowAsync(uc);
        }
    }
}