using Windows.UI.Xaml.Controls;

namespace MyerSplashCustomControl.Adapter
{
    public interface IListViewAdapter
    {
        void OnAttachToListView(ListViewBase listView);
        void OnChoosingItemContainer(ListViewBase sender, ChoosingItemContainerEventArgs args);
        void OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args);
    }
}
