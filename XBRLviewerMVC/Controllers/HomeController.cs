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
using System.Web;
using Microsoft.AspNetCore.Hosting;
using XBRLviewerMVC.ViewModels;
using Newtonsoft.Json.Linq;
using System.Text;

namespace XBRLviewerMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env)
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fo");
            _logger = logger;
            _env = env;
        }

        public IActionResult FileUpload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UploadFile(FileUpload fileUpload)
        {
            if (fileUpload.FormFile != null)
            {
                string filePath = $"{_env.WebRootPath}/data/20191101/" + $"{fileUpload.FormFile.FileName}";

                using (var stream = System.IO.File.Create(filePath))
                {
                    fileUpload.FormFile.CopyTo(stream);
                }

                var xbrlInstance = new XbrlDocument();
                xbrlInstance.Load(filePath); // Does not get the facts, if a file with the same name is already in the directory.
                List<object> factsList = XBRLservices.GepsioDataExtractor.GetValuesFromFactsToList(xbrlInstance);
                List<object> contextsList = XBRLservices.GepsioDataExtractor.GetValuesFromContextToList(xbrlInstance);
                var factsJson = JsonConvert.SerializeObject(factsList);
                var contextJson = JsonConvert.SerializeObject(contextsList);

                string fileName = filePath.Substring(filePath.LastIndexOf("/") + 1, filePath.Substring(filePath.LastIndexOf("/")).Length - filePath.Substring(filePath.LastIndexOf(".")).Length - 1);

                System.IO.File.WriteAllText($"{_env.WebRootPath}/data/json/" + fileName + ".json", "{" + "\"facts\":" + factsJson + "," + "\"contexts\":" + contextJson + "}");
            }
            
            return Redirect(" / ");
        }

        public IActionResult XBRLviewer()
        {
            string json = XBRLservices.JSONServices.Read("facts.json", "data");

            JObject jObject = JObject.Parse(json);

            JArray factsJson = (JArray)jObject["facts"];
            JArray contextsJson = (JArray)jObject["contexts"];

            XBRLViewerViewModel palavm = new XBRLViewerViewModel();
            palavm.Facts = factsJson.ToObject<List<FactModel>>();
            palavm.Contexts = contextsJson.ToObject<List<ContextModel>>();

            return View(palavm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
