using Microsoft.Extensions.Configuration;
using RepoDb;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
    public class RepoSQLDBConnection : IConnection
    {
        public IDbConnection _dbconn { get; private set; }
        public IDbTransaction _tran { get; set; }
        private IConfiguration _config { get; set; }
        private IConfigurationSection _appsettings { get; set; }

        public RepoSQLDBConnection(IConfiguration config)
        {
            _config = config;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool isDisposed)
        {
            if (isDisposed)
            {
                if (this._tran != null)
                {
                    this._tran.Dispose();
                    this._tran = null;
                }

                if (this._dbconn != null)
                {
                    this._dbconn.Dispose();
                    this._dbconn.Close();
                    this._dbconn = null;
                }
            }
        }

        public void OpenConnection(string connString)
        {
            var conn = _config.GetConnectionString(connString);
            this._dbconn = new SqlConnection(conn);
            this._dbconn.Open();
            SqlServerBootstrap.Initialize();
        }

        public string GetAppSettings(string key)
        {
            this._appsettings = _config.GetSection("AppSettings");
            return _appsettings.GetSection(key).Value;
        }
    }
}
