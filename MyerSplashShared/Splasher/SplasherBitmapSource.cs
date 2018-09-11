using System.ComponentModel;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace MyerSplashShared.Splasher
{
    public class SplasherBitmapSource : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private BitmapImage _bitmap;
        [IgnoreDataMember]
        public BitmapImage Bitmap
        {
            get
            {
                return _bitmap;
            }
            set
            {
                if (_bitmap != value)
                {
                    _bitmap = value;
                    RaisePropertyChanged(nameof(Bitmap));
                }
            }
        }

        private SplasherController _controller = new SplasherController();

        public SplasherBitmapSource()
        {
        }

        public async Task SetImageUriAsync(string uri)
        {
            var request = new ImageRequest(uri);
            var bm = await _controller.FetchBitmapImageAsync(request);
            Bitmap = bm;
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}