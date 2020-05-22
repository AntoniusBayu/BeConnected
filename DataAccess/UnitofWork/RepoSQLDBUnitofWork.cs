using System.Data;

namespace DataAccess
{
    public class RepoSQLDBUnitofWork : IUnitofWork
    {
        public RepoSQLDBConnection _repodbConn { get; private set; }
        public RepoSQLDBUnitofWork(IConnection _conn)
        {
            _repodbConn = (RepoSQLDBConnection)_conn;
        }
        public void OpenConnection(string connString)
        {
            if (_repodbConn._dbconn == null)
            {
                _repodbConn.OpenConnection(connString);
            }
        }
        public void BeginTransaction()
        {
            _repodbConn._tran = _repodbConn._dbconn.BeginTransaction(IsolationLevel.ReadUncommitted);
        }

        public void CommitTransaction()
        {
            if (_repodbConn._tran.Connection != null)
            {
                _repodbConn._tran.Commit();
            }
        }

        public void Dispose()
        {
            _repodbConn.Dispose();
        }

        public void RollbackTransaction()
        {
            if (_repodbConn._tran.Connection != null)
            {
                _repodbConn._tran.Rollback();
            }
        }

        public string GetAppSettings(string key)
        {
            return _repodbConn.GetAppSettings(key);
        }
    }
}
