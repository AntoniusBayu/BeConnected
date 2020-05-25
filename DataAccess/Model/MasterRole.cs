using RepoDb.Attributes;

namespace DataAccess
{
    [Map("MasterRole")]
    public class MasterRole : BaseModel
    {
        [Primary]
        public string RoleID { get; set; }
        public string RoleDescription { get; set; }
    }
}
