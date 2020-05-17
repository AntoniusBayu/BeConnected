using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataAccess
{
    public interface IRepository<T> where T : class
    {
        IList<T> ReadByLambda(Expression<Func<T, bool>> lambda);
        string QuerySelect();
        IList<T> ReadByQuery(string sqlQuery, object parameter);
        void Insert(T data);
        void Insert(IList<T> data);
        void Update(T data);
        void Delete(T data);
        void AuditTrail(string statusCode, T data);
        void AuditTrail(string statusCode, IList<T> data);
    }
}
