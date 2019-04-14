using Newtonsoft.Json;

namespace MyerSplash.Data
{
    public class UnsplashUser : ModelBase
    {
        public bool AuthorChanged { get; set; } = false;

        private string _name;
        [JsonProperty("name")]
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    RaisePropertyChanged(() => Name);
                }
            }
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("links")]
        private Links _links;

        public Links Links
        {
            get => _links;
            set
            {
                if (_links != value)
                {
                    _links = value;
                    RaisePropertyChanged(() => Links);
                }
            }
        }

        private string _bio;
        [JsonProperty("bio")]
        public string Bio
        {
            get => _bio;
            set
            {
                if (_bio != value)
                {
                    _bio = value;
                    RaisePropertyChanged(() => Bio);
                }
            }
        }
    }
}