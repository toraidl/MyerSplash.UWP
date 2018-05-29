using MyerSplash.View.Uc;
using System;
using System.Numerics;
using Windows.ApplicationModel;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyerSplash.Common
{
    public class PresentedArgs
    {
        public bool Presented { get; set; }

        public PresentedArgs(bool presented)
        {
            Presented = presented;
        }
    }

    public class NavigableUserControl : UserControl, INavigableUserControl
    {
        private bool IsInWideWindow
        {
            get
            {
                var ratio = Window.Current.Bounds.Width / Window.Current.Bounds.Height;
                return ratio > 1;
            }
        }

        public bool Presented
        {
            get { return (bool)GetValue(PresentedProperty); }
            set { SetValue(PresentedProperty, value); }
        }

        public static readonly DependencyProperty PresentedProperty =
            DependencyProperty.Register("Presented", typeof(bool), typeof(NavigableUserControl),
                new PropertyMetadata(false, OnPresentedPropertyChanged));

        public event EventHandler<PresentedArgs> OnPresentedChanged;

        private static void OnPresentedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as INavigableUserControl;
            if ((bool)e.NewValue)
            {
                control.OnPresented();
            }
            else
            {
                control.OnHide();
            }

            control.ToggleAnimation();
        }

        private Compositor _compositor;
        private Visual _rootVisual;

        public NavigableUserControl()
        {
            if (!DesignMode.DesignModeEnabled)
            {
                InitComposition();
                this.SizeChanged += UserControlBase_SizeChanged;
            }
        }

        private void UserControlBase_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResetOffset();
        }

        private void InitComposition()
        {
            _compositor = this.GetVisual().Compositor;
            _rootVisual = this.GetVisual();
            ResetOffset();
        }

        private void ResetOffset()
        {
            if (!Presented)
            {
                if (IsInWideWindow)
                {
                    _rootVisual.SetTranslation(new Vector3(0f, (float)this.ActualHeight, 0f));
                }
                else
                {
                    _rootVisual.SetTranslation(new Vector3((float)this.ActualWidth, 0f, 0f));
                }
            }
        }

        public virtual void OnHide()
        {
            OnPresentedChanged?.Invoke(this, new PresentedArgs(false));
        }

        public virtual void OnPresented()
        {
            OnPresentedChanged?.Invoke(this, new PresentedArgs(true));
        }

        public void ToggleAnimation()
        {
            var offsetAnimation = _compositor.CreateScalarKeyFrameAnimation();
            offsetAnimation.InsertKeyFrame(1f, Presented ? 0f :
                (IsInWideWindow ? (float)this.ActualHeight : (float)this.ActualWidth));
            offsetAnimation.Duration = TimeSpan.FromMilliseconds(800);
           
            _rootVisual.StartAnimation(IsInWideWindow ? _rootVisual.GetTranslationYPropertyName()
                : _rootVisual.GetTranslationXPropertyName(), offsetAnimation);
        }
    }
}