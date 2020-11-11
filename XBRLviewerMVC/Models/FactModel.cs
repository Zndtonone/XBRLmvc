using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XBRLviewerMVC.Models
{
    public class FactModel
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public string Value { get; set; }
        public string Namespace { get; set; }
        public string ContextID { get; set; }
    }
}
