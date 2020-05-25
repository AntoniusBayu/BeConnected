using RepoDb.Attributes;

namespace DataAccess
{
    [Map("MasterCompany")]
    public class MasterCompany : BaseModel
    {
        [Primary]
        public string CompanyID { get; set; }
        public string CompanyName { get; set; }
        public bool IsVerified { get; set; }
    }
}
