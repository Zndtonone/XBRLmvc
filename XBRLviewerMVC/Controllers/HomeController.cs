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
                xbrlInstance.Load(filePath); // Fær ikki facts um sama fíla longu liggur á staðnum.
                List<object> listToConvert = XBRLservices.GepsioDataExtractor.GetAllValuesFromFactsList(xbrlInstance);
                var json = JsonConvert.SerializeObject(listToConvert);
                // FileName - .xml og leggja .json afturat
                //System.IO.File.WriteAllText($"{_env.WebRootPath}/data/json/" + $"{fileUpload.FormFile.FileName}", json);
            }
            
            return Redirect("/");
        }

        public IActionResult XBRLviewer()
        {
            // Creating new list of FactModel
            List<FactModel> facts = JsonConvert.DeserializeObject<List<FactModel>>(XBRLservices.JSONServices.Read("facts.json", "data"));

            return View(facts);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
