using System.Collections.Generic;
using System.Linq;

namespace DataAccess
{
    public class MasterCompanyRepository : RepoSQLDBRepository<MasterCompany>
    {
        public MasterCompanyRepository(IUnitofWork uow) : base(uow)
        { }

        public MasterCompany GetSingleData(MasterCompany param)
        {
            IDictionary<string, object> SQLparam = new Dictionary<string, object>();

            string sqlQuery = base.QuerySelect();

            if (!string.IsNullOrEmpty(param.CompanyID))
            {
                SQLparam.Add("CompanyID", param.CompanyID);
                sqlQuery += " AND CompanyID = @CompanyID ";
            }

            SQLparam.Add("IsActive", param.IsActive);
            sqlQuery += " AND IsActive = @IsActive ";

            return base.ReadByQuery(sqlQuery, SQLparam).FirstOrDefault();
        }
    }
}
