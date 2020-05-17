using RepoDb.Attributes;

namespace DataAccess
{
    [Map("MasterUserRole")]
    public class MasterUserRole : BaseModel
    {
        public string UserRoleID { get; set; }
        public string RoleID { get; set; }
        public string UserID { get; set; }
    }
}
