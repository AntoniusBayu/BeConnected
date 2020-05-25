using RepoDb.Attributes;

namespace DataAccess
{
    [Map("MasterUserEducation")]
    public class MasterUserEducation : BaseModel
    {
        [Primary]
        public string EducationID { get; set; }
        public string UserID { get; set; }
        public string EducationAt { get; set; }
        public int YearPeriodFrom { get; set; }
        public int MonthPeriodFrom { get; set; }
        public int YearPeriodTo { get; set; }
        public int MonthPeriodTo { get; set; }
        public string Description { get; set; }
        public bool CurrentEducation { get; set; }
    }
}
