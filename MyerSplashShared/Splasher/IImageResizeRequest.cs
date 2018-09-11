using System;

namespace MyerSplashShared.Splasher
{
    public interface IImageResizeOption
    {
        Tuple<int, int> GetResizeSize(int originalWidth, int originalHeight);
    }
}
