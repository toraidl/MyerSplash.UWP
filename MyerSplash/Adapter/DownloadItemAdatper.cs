using MyerSplash.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace MyerSplash.Adapter
{
    public class DownloadItemAdatper : AnimatedAdapter
    {
        public async override void OnChoosingItemContainer(ListViewBase sender, ChoosingItemContainerEventArgs args)
        {
            if ((args.Item is DownloadItem item))
            {
                await item.ImageItem.TryLoadBitmapAsync();
            }
        }
    }
}
