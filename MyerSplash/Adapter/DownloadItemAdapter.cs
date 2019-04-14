using MyerSplash.Model;
using Windows.UI.Xaml.Controls;

namespace MyerSplash.Adapter
{
    public class DownloadItemAdapter : AnimatedAdapter
    {
        public override async void OnChoosingItemContainer(ListViewBase sender, ChoosingItemContainerEventArgs args)
        {
            if ((args.Item is DownloadItem item))
            {
                await item.ImageItem.TryLoadBitmapAsync();
            }
        }
    }
}
