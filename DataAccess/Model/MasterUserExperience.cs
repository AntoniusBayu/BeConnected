using RepoDb.Attributes;

namespace DataAccess
{
    [Map("MasterUserExperience")]
    public class MasterUserExperience : BaseModel
    {
        public string ExperienceID { get; set; }
        public string UserID { get; set; }
        public string CompanyName { get; set; }
        public string Position { get; set; }
        public int YearPeriodFrom { get; set; }
        public int MonthPeriodFrom { get; set; }
        public int YearPeriodTo { get; set; }
        public int MonthPeriodTo { get; set; }
        public string Description { get; set; }
        public bool CurrentCompany { get; set; }
    }
}
