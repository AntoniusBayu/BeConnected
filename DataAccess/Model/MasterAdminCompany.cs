using RepoDb.Attributes;

namespace DataAccess
{
    [Map("MasterAdminCompany")]
    public class MasterAdminCompany : BaseModel
    {
        public string AdminID { get; set; }
        public string UserID { get; set; }
        public string CompanyID { get; set; }
        public bool IsVerified { get; set; }
    }
}
