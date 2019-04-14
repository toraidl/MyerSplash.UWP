using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

namespace MyerSplash.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<DownloadsViewModel>(true);
            SimpleIoc.Default.Register<MainViewModel>(true);
        }

        public MainViewModel MainVM => SimpleIoc.Default.GetInstance<MainViewModel>();

        public DownloadsViewModel DownloadsVM => SimpleIoc.Default.GetInstance<DownloadsViewModel>();
    }
}