using RepoDb;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess
{
    public abstract class RepoSQLDBRepository<T> : IRepository<T> where T : class
    {
        private RepoSQLDBUnitofWork _uow { get; set; }
        public RepoSQLDBRepository(IUnitofWork uow)
        {
            this._uow = (RepoSQLDBUnitofWork)uow;
        }
        public void AuditTrail(string statusCode, T data)
        {
            throw new System.NotImplementedException();
        }

        public void AuditTrail(string statusCode, IList<T> data)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(T data)
        {
            this._uow._repodbConn._dbconn.Delete(data, transaction: _uow._repodbConn._tran);
        }

        public void Insert(T data)
        {
            this._uow._repodbConn._dbconn.Insert(data, transaction: _uow._repodbConn._tran);
        }

        public void Insert(IList<T> data)
        {
            this._uow._repodbConn._dbconn.InsertAll(data, batchSize: data.Count, transaction: _uow._repodbConn._tran);
        }

        public string QuerySelect()
        {
            string str = "SELECT * FROM " + typeof(T).Name + " WITH(NOLOCK) WHERE 1=1 ";

            return str;
        }

        public IList<T> ReadByLambda(Expression<System.Func<T, bool>> lambda)
        {
            return this._uow._repodbConn._dbconn.Query<T>(lambda).ToList();
        }

        public IList<T> ReadByQuery(string sqlQuery, object parameter)
        {
            return this._uow._repodbConn._dbconn.ExecuteQuery<T>(sqlQuery, param: parameter).ToList();
        }

        public void Update(T data)
        {
            this._uow._repodbConn._dbconn.Update(data, transaction: this._uow._repodbConn._tran);
        }
    }
}
