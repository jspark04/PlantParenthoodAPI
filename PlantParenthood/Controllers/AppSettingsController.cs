using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PlantParenthood.Models;

namespace PlantParenthood.Controllers
{
    public class AppSettingsController : ApiController
    {
        private PlantParenthoodContext db = new PlantParenthoodContext();

        // GET: api/AppSettings
        [ActionName("GetAppSettings")]
        [HttpGet]
        public IQueryable<AppSettings> GetAppSettings()
        {
            return db.AppSettings;
        }

        // Custom route for Kevin
        //[Route("SensorAddResult")]
        [ActionName("UpdateAppSettings")]
        [HttpGet]
        public IHttpActionResult UpdateAppSettings(int appsettingsid, string name, string value)
        {
            /*
            // float SoilMoisture, float Light, float Temperature, float Humidity
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Need to know what the current plant is in order to log it correctly
            var currentPlant = (from entry in db.CareDatas
                                where entry.Current == true
                                select entry).FirstOrDefault();
            int currentCareInfoID = currentPlant.CareInfoID;
            

            db.SensorDatas.Add(sensorData);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = sensorData.SensorDataID }, sensorData);*/

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Create new sensordata object containing complete information to write into db
            AppSettings appSettingsEntryToBeUpdated = new AppSettings
            {
                AppSettingsID = appsettingsid,
                Name = name,
                Value = value
            };

            db.Entry(appSettingsEntryToBeUpdated).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch { 
}

            return Ok(appSettingsEntryToBeUpdated);
        }

        /*
        // GET: api/AppSettings/5
        [ResponseType(typeof(AppSettings))]
        public IHttpActionResult GetAppSettings(int id)
        {
            AppSettings appSettings = db.AppSettings.Find(id);
            if (appSettings == null)
            {
                return NotFound();
            }

            return Ok(appSettings);
        }
        */
        /*
        // PUT: api/AppSettings/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAppSettings(int id, AppSettings appSettings)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != appSettings.AppSettingsID)
            {
                return BadRequest();
            }

            db.Entry(appSettings).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppSettingsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/AppSettings
        [ResponseType(typeof(AppSettings))]
        public IHttpActionResult PostAppSettings(AppSettings appSettings)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AppSettings.Add(appSettings);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = appSettings.AppSettingsID }, appSettings);
        }

        // DELETE: api/AppSettings/5
        [ResponseType(typeof(AppSettings))]
        public IHttpActionResult DeleteAppSettings(int id)
        {
            AppSettings appSettings = db.AppSettings.Find(id);
            if (appSettings == null)
            {
                return NotFound();
            }

            db.AppSettings.Remove(appSettings);
            db.SaveChanges();

            return Ok(appSettings);
        }
        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AppSettingsExists(int id)
        {
            return db.AppSettings.Count(e => e.AppSettingsID == id) > 0;
        }
        */
    }
}