using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq;
using XBRLviewerMVC.Models;

namespace XBRLviewerMVC.ViewModels
{
    public class XBRLViewerViewModel
    {
        public IEnumerable<FactModel> Facts { get; set; }
        public IEnumerable<ContextModel> Contexts { get; set; }
    }
}
