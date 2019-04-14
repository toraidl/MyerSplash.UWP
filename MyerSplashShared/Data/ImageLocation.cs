using Newtonsoft.Json;

namespace MyerSplash.Data
{
    public class ImageLocation : ModelBase
    {
        private string _city;
        [JsonProperty("city")]
        public string City
        {
            get => _city;
            set
            {
                if (_city != value)
                {
                    _city = value;
                    RaisePropertyChanged(() => City);
                }
            }
        }

        private string _country;
        [JsonProperty("country")]
        public string Country
        {
            get => _country;
            set
            {
                if (_country != value)
                {
                    _country = value;
                    RaisePropertyChanged(() => Country);
                }
            }
        }
    }
}