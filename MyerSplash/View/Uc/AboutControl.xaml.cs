using MyerSplash.Common;
using MyerSplash.ViewModel;
using Windows.UI.Xaml;

namespace MyerSplash.View.Uc
{
    public sealed partial class AboutControl : NavigableUserControl
    {
        private AboutViewModel AboutVM { get; }

        public AboutControl()
        {
            this.InitializeComponent();
            this.DataContext = AboutVM = new AboutViewModel();
        }

        public override void OnPresented()
        {
            base.OnPresented();
            Window.Current.SetTitleBar(DummyTitleBar);
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Presented = false;
        }
    }
}