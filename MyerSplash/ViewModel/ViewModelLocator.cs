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

        public MainViewModel MainVM
        {
            get
            {
                return SimpleIoc.Default.GetInstance<MainViewModel>();
            }
        }

        public DownloadsViewModel DownloadsVM
        {
            get
            {
                return SimpleIoc.Default.GetInstance<DownloadsViewModel>();
            }
        }

        public static void Cleanup()
        {
        }
    }
}