using MyerSplashShared.Data;
using Newtonsoft.Json;
using System;

namespace MyerSplash.Data
{
    public class UnsplashImage : ModelBase
    {
        public string ID { get; set; }

        [JsonProperty("urls")]
        public UnsplashUrl Urls { get; set; }

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    RaisePropertyChanged(() => Title);
                }
            }
        }

        [JsonProperty("color")]
        public string ColorValue { get; set; }

        private double _width;
        [JsonProperty("width")]
        public double Width
        {
            get => _width;
            set
            {
                if (_width != value)
                {
                    _width = value;
                    RaisePropertyChanged(() => Width);
                }
            }
        }

        private double _height;
        [JsonProperty("height")]
        public double Height
        {
            get => _height;
            set
            {
                if (_height != value)
                {
                    _height = value;
                    RaisePropertyChanged(() => Height);
                }
            }
        }

        [JsonProperty("links")]
        public UnsplashLinks Links;

        private ImageExif _exif;
        [JsonProperty("exif")]
        public ImageExif Exif
        {
            get => _exif;
            set
            {
                if (_exif != value)
                {
                    _exif = value;
                    RaisePropertyChanged(() => Exif);
                }
            }
        }

        private ImageLocation _location;
        [JsonProperty("location")]
        public ImageLocation Location
        {
            get => _location;
            set
            {
                if (_location != value)
                {
                    _location = value;
                    RaisePropertyChanged(() => Location);
                }
            }
        }

        private UnsplashUser _owner;
        [JsonProperty("user")]
        public UnsplashUser Owner
        {
            get => _owner;
            set
            {
                if (_owner != value)
                {
                    _owner = value;
                    RaisePropertyChanged(() => Owner);
                }
            }
        }

        private bool _liked;
        [JsonProperty("liked_by_user")]
        public bool Liked
        {
            get => _liked;
            set
            {
                if (_liked != value)
                {
                    _liked = value;
                    RaisePropertyChanged(() => Liked);
                }
            }
        }

        private int _likes;
        [JsonProperty("likes")]
        public int Likes
        {
            get => _likes;
            set
            {
                if (_likes != value)
                {
                    _likes = value;
                    RaisePropertyChanged(() => Likes);
                }
            }
        }

        [JsonProperty("created_at")]
        public string CreateTimeString { get; set; }

        public DateTime CreateTime
        {
            get
            {
                DateTime.TryParse(CreateTimeString, out DateTime time);
                return time;
            }
        }

        public string SimpleCreateTimeString => CreateTime.ToString("yyyy-MM-dd hh-mm-ss");

        private bool _isUnsplash;
        public bool IsUnsplash
        {
            get => _isUnsplash;
            set
            {
                if (_isUnsplash != value)
                {
                    _isUnsplash = value;
                    RaisePropertyChanged(() => IsUnsplash);
                }
            }
        }

        private bool _isInHighlightList;
        public bool IsInHighlightList
        {
            get => _isInHighlightList;
            set
            {
                if (_isInHighlightList != value)
                {
                    _isInHighlightList = value;
                    RaisePropertyChanged(() => IsInHighlightList);
                }
            }
        }

        public UnsplashImage()
        {
            IsUnsplash = true;
            IsInHighlightList = false;
        }
    }
}