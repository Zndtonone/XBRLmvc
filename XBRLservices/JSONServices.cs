﻿using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;

namespace XBRLservices
{
    public class JSONServices
    {
        public JSONServices() { }

        // RTH - Read JSON file from wwwroot - Something added for git
        public static string Read(string fileName, string location)
        {
            string root = "wwwroot";
            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                root,
                location,
                fileName);

            string jsonResult;

            using (StreamReader streamReader = new StreamReader(path, System.Text.Encoding.UTF8, true))
            {
                jsonResult = streamReader.ReadToEnd();
            }

            return jsonResult;
        }

        // RTH - Write JSON file to location
        public static void Write(string fileName, string location, string jSONString)
        {
            string root = "wwwroot";
            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                root,
                location,
                fileName);

            using (var streamWriter = File.CreateText(path))
            {
                streamWriter.Write(jSONString);
            }
        }

        // RTH - Convert to JSON and write to file from List
        public static void JsonWriteToFileFromList(List<object> listToConvert)
        {
            var output = Newtonsoft.Json.JsonConvert.SerializeObject(listToConvert);

            File.WriteAllText(@"c:\GepsioJSON\myObj.json", output);
        }

        // RTH - Convert to JSON and write to file from List
        public static string JsonConvert(List<object> listToConvert)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(listToConvert);

            return json;
        }
    }
}
