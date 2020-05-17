using RepoDb.Attributes;

namespace DataAccess
{
    [Map("MasterRole")]
    public class MasterRole : BaseModel
    {
        public string RoleID { get; set; }
        public string RoleDescription { get; set; }
    }
}
