using MyerSplash.Common;
using System;
using System.Numerics;
using Windows.UI.Composition;
using Windows.UI.Xaml;

namespace MyerSplash.View.Uc
{
    public sealed partial class GestureHintControl
    {
        private CompositionAnimation _animation;
        public event EventHandler<EventArgs> OnClickToDismiss;
        private Visual _iconVisual;

        public GestureHintControl()
        {
            this.InitializeComponent();
        }

        public void StartAnimation()
        {
            if (_animation != null)
            {
                return;
            }
            _iconVisual = GestureIcon.GetVisual();
            var comp = _iconVisual.Compositor;
            var animation = comp.CreateVector3KeyFrameAnimation();
            animation.InsertKeyFrame(0f, new Vector3(0f, 0f, 0f));
            animation.InsertKeyFrame(0.5f, new Vector3(0f, 80f, 0f));
            animation.InsertKeyFrame(1f, new Vector3(0f, 0f, 0f));
            animation.Duration = TimeSpan.FromMilliseconds(2000);
            animation.Direction = AnimationDirection.Normal;
            animation.IterationBehavior = AnimationIterationBehavior.Forever;
            _iconVisual.StartAnimation("Translation", animation);
            _animation = animation;
        }

        public void StopAnimation()
        {
            _animation = null;
            _iconVisual.StopAnimation("Translation");
        }

        private void GotIt_Click(object sender, RoutedEventArgs e)
        {
            OnClickToDismiss?.Invoke(this, new EventArgs());
        }
    }
}
