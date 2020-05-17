using System.Collections.Generic;
using System.Linq;

namespace DataAccess
{
    public class MasterUserRepository : RepoSQLDBRepository<MasterUser>
    {
        public MasterUserRepository(IUnitofWork uow) : base(uow)
        { }

        public MasterUser GetSingleData(MasterUser param)
        {
            IDictionary<string, object> SQLparam = new Dictionary<string, object>();
            MasterUser data = new MasterUser();

            string sqlQuery = base.QuerySelect();

            if (!string.IsNullOrEmpty(param.UserName))
            {
                SQLparam.Add("UserName", param.UserName);
                sqlQuery += " AND UserName = @UserName ";
            }

            //SQLparam.Add("IsActive", param.IsActive);
            //sqlQuery += " AND IsActive = @IsActive ";

            return base.ReadByQuery(sqlQuery, SQLparam).FirstOrDefault();
        }
    }
}
