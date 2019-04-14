using Newtonsoft.Json;

namespace MyerSplash.Data
{
    public class ImageExif : ModelBase
    {
        private const string DEFAULT = "Unknown";

        private string _model;
        [JsonProperty("model")]
        public string Model
        {
            get => _model;
            set
            {
                if (_model != value)
                {
                    _model = value;
                    RaisePropertyChanged(() => Model);
                }
            }
        }

        private string _exposureTime;
        [JsonProperty("exposure_time")]
        public string ExposureTime
        {
            get => $"{_exposureTime}s";
            set
            {
                if (_exposureTime != value)
                {
                    _exposureTime = value;
                    RaisePropertyChanged(() => ExposureTime);
                }
            }
        }

        private string _aperture;
        [JsonProperty("aperture")]
        public string Aperture
        {
            get => $"f/{_aperture}";
            set
            {
                if (_aperture != value)
                {
                    _aperture = value;
                    RaisePropertyChanged(() => Aperture);
                }
            }
        }

        private int? _iso;
        [JsonProperty("iso")]
        public int? Iso
        {
            get => _iso;
            set
            {
                if (_iso != value)
                {
                    _iso = value;
                    RaisePropertyChanged(() => Iso);
                    RaisePropertyChanged(() => IsoString);
                }
            }
        }

        public string IsoString => Iso == null ? DEFAULT : Iso.ToString();

        public ImageExif()
        {
            Model = DEFAULT;
            ExposureTime = DEFAULT;
            Aperture = DEFAULT;
        }
    }
}