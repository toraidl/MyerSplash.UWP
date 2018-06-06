using MyerSplash.Model;
using Windows.UI.Xaml.Controls;

namespace MyerSplash.Adapter
{
    public class UnsplashImageAdapter : AnimatedAdapter
    {
        public async override void OnChoosingItemContainer(ListViewBase sender, ChoosingItemContainerEventArgs args)
        {
            if ((args.Item is ImageItem imageItem))
            {
                await imageItem.TryLoadBitmapAsync();
            }
        }
    }
}
