using RepoDb.Attributes;
using System;

namespace DataAccess
{
    [Map("TrxApplicant")]
    public class TrxApplicant : BaseModel
    {
        [Primary]
        public string ApplicantID { get; set; }
        public string JobID { get; set; }
        public string UserID { get; set; }
        public string UserReffID { get; set; }
        public DateTime SubmittedDate { get; set; }
    }
}
