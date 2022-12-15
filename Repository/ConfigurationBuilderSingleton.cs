using Microsoft.Extensions.Configuration;


namespace Repository
{
    public class ConfigurationBuilderSingleton
    {
        private static ConfigurationBuilderSingleton _instance = null;
        private static readonly object _instanceLock = new object();
        public static IConfigurationRoot _configuration;

        private ConfigurationBuilderSingleton()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
             
            _configuration= builder.Build();
        }
        public static ConfigurationBuilderSingleton Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new ConfigurationBuilderSingleton();
                    }
                    return _instance;
                }
            }
        }
        public static IConfigurationRoot ConfigurationRoot
        {
            get
            {
                if (_configuration == null)
                {
                    var x = Instance;
                }
                return _configuration;
            }
        }
    }
}
