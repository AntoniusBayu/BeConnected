using RepoDb.Attributes;

namespace DataAccess
{
    [Map("MasterCompany")]
    public class MasterCompany : BaseModel
    {
        public string CompanyID { get; set; }
        public string CompanyName { get; set; }
        public bool IsVerified { get; set; }
    }
}
