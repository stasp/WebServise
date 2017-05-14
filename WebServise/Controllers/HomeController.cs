using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
using WebServise.Models;

namespace WebServise.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Agrigation(IncomingData getData)
        {
            Read Read = new Read();
            string[] fileContents = Directory.GetFiles(Server.MapPath(@"~/App_Data/"));
            if (Read.readFile(getData.Mode, fileContents))
            {
                List<DataValue> ListData = Read.ListValue.Where(x =>
                                                                x.date >= DateTime.ParseExact(getData.DateStart, "yyyyMMdd", CultureInfo.GetCultureInfo("tr-TR")) &
                                                                x.date <= DateTime.ParseExact(getData.DateEnd, "yyyyMMdd", CultureInfo.GetCultureInfo("tr-TR"))).ToList<DataValue>();
                if (ListData.Count() > 0)
                {
                    Dictionary<string, Dictionary<string, double>> dic = new Dictionary<string, Dictionary<string, double>>();
                    Dictionary<string, object> result = new Dictionary<string, object>();

                    foreach (var item in ListData.GroupBy(x => x.code).Select(x => x.First()))
                    {
                        dic[item.code.ToLower()] = new Dictionary<string, double>();
                        dic[item.code.ToLower()]["rate"] = ListData.Where(x => x.code == item.code).Average(x => x.rate);
                    }

                    result["rates"] = new object[] { dic };
                    string json = new JavaScriptSerializer().Serialize(result);
                    ViewBag.jval = json;
                }
            }
            return View();
        }
    }
}