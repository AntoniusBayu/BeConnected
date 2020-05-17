using System;

namespace DataAccess
{
    public interface IUnitofWork : IConnection, IDisposable
    {
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}
