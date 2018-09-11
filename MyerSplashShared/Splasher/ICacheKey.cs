using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyerSplashShared.Splasher
{
    public interface ICacheKey
    {
        void GenerateKey(ImageRequest request);

        string Key { get; }
    }
}
