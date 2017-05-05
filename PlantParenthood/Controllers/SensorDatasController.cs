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
    public class SensorDatasController : ApiController
    {
        private PlantParenthoodContext db = new PlantParenthoodContext();

        // GET: api/SensorDatas
        public IQueryable<SensorData> GetSensorDatas()
        {
            return db.SensorDatas;
        }

        // GET: api/SensorDatas/5
        [ResponseType(typeof(SensorData))]
        public IHttpActionResult GetSensorData(int id)
        {
            SensorData sensorData = db.SensorDatas.Find(id);
            if (sensorData == null)
            {
                return NotFound();
            }

            return Ok(sensorData);
        }

        // PUT: api/SensorDatas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSensorData(int id, SensorData sensorData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sensorData.SensorDataID)
            {
                return BadRequest();
            }

            db.Entry(sensorData).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SensorDataExists(id))
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

        // POST: api/SensorDatas
        [ResponseType(typeof(SensorData))]
        public IHttpActionResult PostSensorData(SensorData rawData)
        {
            //float SoilMoisture, float Light, float Temperature, float Humidity
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Need to know what the current plant is in order to log it correctly
            var currentPlant = (from entry in db.CareDatas
                               where entry.Current == true
                               select entry).FirstOrDefault();
            int currentCareInfoID = currentPlant.CareInfoID;
            
            // Create new sensordata object containing complete information to write into db
            SensorData sensorData = new SensorData
            {
                SoilMoisture = rawData.SoilMoisture,
                Light = rawData.Light,
                Temperature = rawData.Temperature,
                Humidity = rawData.Humidity,
                CreatedDate = DateTime.Now,
                CareInfoID = currentCareInfoID
            };

            db.SensorDatas.Add(sensorData);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = sensorData.SensorDataID }, sensorData);
        }

        // DELETE: api/SensorDatas/5
        [ResponseType(typeof(SensorData))]
        public IHttpActionResult DeleteSensorData(int id)
        {
            SensorData sensorData = db.SensorDatas.Find(id);
            if (sensorData == null)
            {
                return NotFound();
            }

            db.SensorDatas.Remove(sensorData);
            db.SaveChanges();

            return Ok(sensorData);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SensorDataExists(int id)
        {
            return db.SensorDatas.Count(e => e.SensorDataID == id) > 0;
        }
    }
}