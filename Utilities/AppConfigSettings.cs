using Microsoft.Extensions.Configuration;
using Utilities.Interfaces;

namespace Utilities
{
    public class AppConfigSettings: ISettings
    {
        private readonly IConfiguration _configuration;

        public AppConfigSettings(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //public string this[string key] => _configuration[key];
        public string this[string key]
        {
            get => _configuration[key];
            set { }
        }
    }
}
