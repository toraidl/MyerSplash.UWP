using MyerSplashCustomControl;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;
using Windows.UI.Core;
using MyerSplashShared.Data;
using System.Collections.Generic;
using Windows.ApplicationModel.Email;
using JP.Utils.Helper;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Windows.UI;
using MyerSplashShared.API;

namespace MyerSplash.View.Uc
{
    public sealed partial class NetworkDiagnosisDialog : UserControl
    {
        private HttpClient _client = new HttpClient();
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private IList<DiagnosisItem> _diagosisItems = new List<DiagnosisItem>();
        private Paragraph _paragraph = new Paragraph();

        public NetworkDiagnosisDialog()
        {
            this.InitializeComponent();

            var tokenSource = new CancellationTokenSource();

            _diagosisItems.Add(new DiagnosisItem(_client, "https://juniperphoton.net/myersplash/wallpapers/thumbs/20180401.jpg", "auto-change wallpaper server"));
            _diagosisItems.Add(new DiagnosisItem(_client, $"https://unsplash.com/photos?client_id={Request.AppKey}&page=1&per_page=30", "unsplash server"));
            _diagosisItems.Add(new DiagnosisItem(_client, "https://www.bing.com"));
            _diagosisItems.Add(new DiagnosisItem(_client, "https://www.google.com"));

            InfoBlock.Blocks.Add(_paragraph);
            var task = StartAsync();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            _cts.Cancel();
            Release();
            PopupService.Instance.TryHide();
        }

        private async Task StartAsync()
        {
            await Task.Run(async () =>
            {
                await ShowProgressAsync();

                foreach (var item in _diagosisItems)
                {
                    await AppendInfoAsync(item.ToString());
                    var result = await item.RunAsync();
                    await AppendInfoAsync(result, item.IsStatusCodeSuccessful ? Colors.Green : Colors.Red);
                    await AppendInfoAsync(string.Empty);
                }

                await HideProgressAsync();
                await ShowReportButtonAsync();
            }, _cts.Token);
        }

        private async Task AppendInfoAsync(string str)
        {
            await AppendInfoAsync(str, Colors.White);
        }

        private async Task AppendInfoAsync(string str, Color color)
        {
            await InfoBlock.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var r = new Run
                {
                    Text = $"{Environment.NewLine}{str}",
                    Foreground = new SolidColorBrush(color)
                };

                _paragraph.Inlines.Add(r);
            });
        }

        private async Task ShowProgressAsync()
        {
            await ProgressRing.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ProgressRing.IsActive = true;
                ProgressRing.Visibility = Visibility.Visible;
            });
        }

        private async Task HideProgressAsync()
        {
            await ProgressRing.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ProgressRing.IsActive = false;
                ProgressRing.Visibility = Visibility.Collapsed;
            });
        }

        private async Task ShowReportButtonAsync()
        {
            await ReportButton.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ReportButton.Visibility = Visibility.Visible;
            });
        }

        private void Release()
        {
            _client.Dispose();
        }

        private async void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            var rec = new EmailRecipient("dengweichao@hotmail.com");
            var mes = new EmailMessage();
            mes.To.Add(rec);

            var result = "";
            foreach (var l in _paragraph.Inlines)
            {
                if (l is Run)
                {
                    result += (l as Run).Text;
                }
            }

            mes.Body = result;
            mes.Subject = $"MyerSplash for Windows 10, {App.GetAppVersion()} feedback, {DeviceHelper.OSVersion}";
            await EmailManager.ShowComposeNewEmailAsync(mes);
        }
    }
}
