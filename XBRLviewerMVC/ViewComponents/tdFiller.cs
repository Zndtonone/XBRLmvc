using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace XBRLviewerMVC.ViewComponents
{
    public class tdFiller
    {
        public tdFiller()
        {

        }

        public IViewComponentResult Invoke()
        {
            return new HtmlContentViewComponentResult(new HtmlString("hello - <b>Yes, Hello</b>"));
        }
    }
}
