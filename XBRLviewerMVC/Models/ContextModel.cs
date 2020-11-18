using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XBRLviewerMVC.Models
{
    public class ContextModel
    {
        public string DurationPeriod { get; set; }
        public string ForeverPeriod { get; set; }
        public string Fragment { get; set; }
        public string Id { get; set; }
        public string Identifier { get; set; }
        public string IdentifierScheme { get; set; }
        public string InstantDate { get; set; }
        public string InstantPeriod { get; set; }
        public string PeriodEndDate { get; set; }
        public string PeriodStartDate { get; set; }
        public string Segment { get; set; }
    }
}
