namespace MyerSplash.View.Uc
{
    public interface INavigableUserControl
    {
        bool Presented { get; set; }

        void OnPresented();

        void OnHide();

        void ToggleAnimation();
    }
}