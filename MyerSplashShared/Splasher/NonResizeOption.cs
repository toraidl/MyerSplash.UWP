using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyerSplashShared.Splasher
{
    public class NonResizeOption : IImageResizeOption
    {
        public Tuple<int, int> GetResizeSize(int originalWidth, int originalHeight)
        {
            return new Tuple<int, int>(originalWidth, originalHeight);
        }
    }
}
