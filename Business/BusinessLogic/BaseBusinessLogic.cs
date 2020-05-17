using Microsoft.Extensions.Configuration;

namespace Business
{
    public abstract class BaseBusinessLogic
    {
        protected IConfigurationSection _Appsettings { get; set; }
        protected IConfiguration _Config { get; set; }
        protected string SQLDBConn { get { return _Config.GetConnectionString("dbConnection"); } }
        public BaseBusinessLogic(IConfiguration config)
        {
            _Config = config;
        }

        protected string GetAppSettings(string key)
        {
            this._Appsettings = _Config.GetSection("AppSettings");
            return _Appsettings.GetSection(key).Value;
        }
    }
}
