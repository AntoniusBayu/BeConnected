using RepoDb.Attributes;
using System;

namespace DataAccess
{
    [Map("AuditLogin")]
    public class AuditLogin : BaseModel
    {
        public string LogID { get; set; }
        public string UserID { get; set; }
        public string IPAddress { get; set; }
        public DateTime LoginDate { get; set; }
    }
}
