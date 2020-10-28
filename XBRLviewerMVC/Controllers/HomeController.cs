using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JeffFerguson.Gepsio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using XBRLviewerMVC.Models;

namespace XBRLviewerMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fo");
            _logger = logger;
        }

        public IActionResult XBRLviewer()
        {
            Response.Headers["Content-Type"] = "charset=utf-8";

            // Creating new list of FactModel
            List<FactModel> facts = new List<FactModel>();

            // Loading XBRL document
            //var xbrlInstance = new XbrlDocument();
            //xbrlInstance.Load(Environment.CurrentDirectory + @"\20191101\2065_1_2019.xml");

            // Converting XBRL instance to JSON
            //List<object> listToConvert = XBRLservices.GepsioDataExtractor.GetAllValuesFromFactsList(xbrlInstance);
            //string jsonToRead = XBRLservices.JSONServices.JsonConvert(listToConvert);

            // Deserializing JSON and loading it to FactModel list
            facts = JsonConvert.DeserializeObject<List<FactModel>>(XBRLservices.JSONServices.Read("facts.json", "data"));

            return View(facts);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
