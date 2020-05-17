using RepoDb.Attributes;
using System;

namespace DataAccess
{
    [Map("TrxListingJob")]
    public class TrxListingJob : BaseModel
    {
        public string JobID { get; set; }
        public string CompanyID { get; set; }
        public string Position { get; set; }
        public string Requirement { get; set; }
        public string JobDesk { get; set; }
        public string Benefit { get; set; }
        public decimal RangeSalaryStart { get; set; }
        public decimal RangeSalaryEnd { get; set; }
        public DateTime PostingDate { get; set; }
        public DateTime ExpiredDate { get; set; }
    }
}
