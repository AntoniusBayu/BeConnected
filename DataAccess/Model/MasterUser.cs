using RepoDb.Attributes;
using System;

namespace DataAccess
{
    [Map("MasterUser")]
    public class MasterUser : BaseModel
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime PasswordExpiredDate { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public DateTime Birthdate { get; set; }
        public string Avatar { get; set; }
        public string UserCV { get; set; }
    }
}
