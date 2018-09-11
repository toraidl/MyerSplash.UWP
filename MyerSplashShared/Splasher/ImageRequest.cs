namespace MyerSplashShared.Splasher
{
    // todo use pool to optimize objects
    public class ImageRequest
    {
        public string Url { get; set; }

        public IImageResizeOption ResizeOption { get; set; }

        public ImageRequest(string url)
        {
            this.Url = url;
            this.ResizeOption = new NonResizeOption();
        }

        public ImageRequest(string url, IImageResizeOption option)
        {
            this.Url = url;
            this.ResizeOption = option;
        }
    }
}
