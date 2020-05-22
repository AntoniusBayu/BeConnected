using System;

namespace DataAccess
{
    public interface IConnection : IDisposable
    {
        void OpenConnection(string ConnString);
        string GetAppSettings(string key);
    }
}
