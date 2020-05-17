using System.Collections.Generic;

namespace DataAccess
{
    public interface IVMRepository<T> where T : class
    {
        IList<T> ReadByQuery(string sqlQuery, object parameter);
    }
}
