using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using System.IO;
using Gardening.Models;
using Newtonsoft.Json;
using Gardening.APIKeys;

namespace Gardening.Controllers
{
    public class PastWeatherController : Controller
    {
       
        public ActionResult Index()
        {

            string startDate = "2019-03-22";
            string endDate = "2019-03-22";
            APIKeyClass keys = new APIKeyClass();
            string NOAAkey = keys.NKey;
            string locationid = keys.locationB;


            string NOAAAPI = "https://www.ncdc.noaa.gov/cdo-web/api/v2/data?datasetid=GHCND&locationid=" + locationid + "&startdate=" + startDate + "&enddate=" + endDate;
            HttpWebRequest WR = WebRequest.CreateHttp(NOAAAPI);
            WR.Headers.Add("token", NOAAkey);
            HttpWebResponse Response = (HttpWebResponse)WR.GetResponse();
            StreamReader rd = new StreamReader(Response.GetResponseStream());
            string data = rd.ReadToEnd();
            rd.Close();

            MetaWrapper AllEvents = JsonConvert.DeserializeObject<MetaWrapper>(data);
            return View();
        }

        // GET: PastWeather/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PastWeather/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PastWeather/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: PastWeather/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PastWeather/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: PastWeather/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PastWeather/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
