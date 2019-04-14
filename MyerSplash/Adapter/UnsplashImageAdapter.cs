using MyerSplash.Model;
using Windows.UI.Xaml.Controls;

namespace MyerSplash.Adapter
{
    public class UnsplashImageAdapter : AnimatedAdapter
    {
        public override async void OnChoosingItemContainer(ListViewBase sender, ChoosingItemContainerEventArgs args)
        {
            if ((args.Item is ImageItem imageItem))
            {
                await imageItem.TryLoadBitmapAsync();
                if (imageItem.Image.IsInHighlightList)
                {
                    await imageItem.LoadAuthorInfoAsync();
                }
            }
        }
    }
}
