using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using MyerSplash.Common;
using MyerSplash.ViewModel;
using Windows.UI.Xaml;

namespace MyerSplash.View.Uc
{
    public sealed partial class ManageDownloadControl : NavigableUserControl
    {
        public DownloadsViewModel DownloadsVM => SimpleIoc.Default.GetInstance<DownloadsViewModel>();

        public RelayCommand CloseCommand
        {
            get => (RelayCommand)GetValue(CloseCommandProperty);
            set => SetValue(CloseCommandProperty, value);
        }

        public static readonly DependencyProperty CloseCommandProperty =
            DependencyProperty.Register("CloseCommand", typeof(RelayCommand), typeof(ManageDownloadControl),
                new PropertyMetadata(null));

        public ManageDownloadControl()
        {
            this.InitializeComponent();
        }

        public void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            CloseCommand?.Execute(null);
        }

        public override void OnPresented()
        {
            base.OnPresented();
            Window.Current.SetTitleBar(DummyTitleBar);
        }
    }
}