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
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace PlantParenthood.Controllers
{
    public class SensorDatasController : ApiController
    {
        private PlantParenthoodContext db = new PlantParenthoodContext();

        // GET: api/SensorDatas/SensorDatas
        [ActionName("GetSensorData")]
        [HttpGet]
        public IQueryable<SensorData> GetSensorDatas()
        {
            var last40PointsOfSensorData = (from entry in db.SensorDatas
                                     orderby entry.CreatedDate descending
                                     select entry).Take(40).OrderBy(p => p.CreatedDate);

            return last40PointsOfSensorData;
        }

        // Custom route for Kevin
        //[Route("SensorAddResult")]
        [ResponseType(typeof(SensorData))]
        [ActionName("PostSensorData")]
        [HttpGet]
        public IHttpActionResult SensorAddResult(float SoilMoisture, float Light, float Temperature, float Humidity)
        {
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

            // Calculate average daily light from this point.
            var endTime = DateTime.Now;
            var startTime = DateTime.Now.AddDays(-1);
            // Getting sensor data over last 24 hours of sensor data
            var currentSensorData = (from entry in db.SensorDatas
                                     where entry.CareInfoID == currentCareInfoID && entry.CreatedDate >= startTime && entry.CreatedDate <= endTime
                                     orderby entry.CreatedDate ascending
                                     select entry);
            // Loop over, estimate light integral using trapezoidal method
            bool isFirstLoop = true;
            float prevLight = 0;
            float lightSum = 0;
            float averagelight = 0;
            var dt = (endTime - startTime).Hours;
            foreach (var datapoint in currentSensorData)
            {
                // Delta T in hours
                dt = (datapoint.CreatedDate - startTime).Hours;
                startTime = datapoint.CreatedDate;
                // Light
                if (isFirstLoop)
                {
                    averagelight = datapoint.Light;
                    prevLight = datapoint.Light;
                    isFirstLoop = false;
                } else
                {
                    averagelight = (datapoint.Light + prevLight)/2;
                    prevLight = datapoint.Light;
                }
                lightSum = lightSum + dt * averagelight;
            }
            // Now add latest point to lightSum
            dt = (endTime - startTime).Hours;
            averagelight = (Light + prevLight) / 2;
            lightSum = lightSum + dt * averagelight;

            // Need to add code to get conditionals
            // Condition key: 1 = Very Low, 2 = Low, 3 = Good, 4 = High, 5 = Very High
            double stretch = 0.1;
            double danger = 0.3;
            int smcond = 3;
            int lcond = 3;
            int tcond = 3;
            int hcond = 3;
            int lsdcond = 3;
            if (SoilMoisture >= currentPlant.SoilMoisture * (1-stretch) && SoilMoisture <= currentPlant.SoilMoisture * (1+stretch))
            {
                smcond = 3;
            } else if (SoilMoisture >= currentPlant.SoilMoisture * (1 - danger) && SoilMoisture <= currentPlant.SoilMoisture * (1 + danger))
            {
                if (SoilMoisture > currentPlant.SoilMoisture)
                {
                    smcond = 4;
                } else
                {
                    smcond = 2;
                }
            } else
            {
                if (SoilMoisture > currentPlant.SoilMoisture)
                {
                    smcond = 5;
                }
                else
                {
                    smcond = 1;
                }
            }
            if (Temperature >= currentPlant.Temperature * (1 - stretch) && Temperature <= currentPlant.Temperature * (1 + stretch))
            {
                tcond = 3;
            }
            else if (Temperature >= currentPlant.Temperature * (1 - danger) && Temperature <= currentPlant.Temperature * (1 + danger))
            {
                if (Temperature > currentPlant.Temperature)
                {
                    tcond = 4;
                }
                else
                {
                    tcond = 2;
                }
            }
            else
            {
                if (Temperature > currentPlant.Temperature)
                {
                    tcond = 5;
                }
                else
                {
                    tcond = 1;
                }
            }
            if (Humidity >= currentPlant.Humidity * (1 - stretch) && Humidity <= currentPlant.Humidity * (1 + stretch))
            {
                hcond = 3;
            }
            else if (Humidity >= currentPlant.Humidity * (1 - danger) && Humidity <= currentPlant.Humidity * (1 + danger))
            {
                if (Humidity > currentPlant.Humidity)
                {
                    hcond = 4;
                }
                else
                {
                    hcond = 2;
                }
            }
            else
            {
                if (Humidity > currentPlant.Humidity)
                {
                    hcond = 5;
                }
                else
                {
                    hcond = 1;
                }
            }
            if (lightSum >= currentPlant.Light * (1 - stretch) && lightSum <= currentPlant.Light * (1 + stretch))
            {
                lsdcond = 3;
            }
            else if (lightSum >= currentPlant.Light * (1 - danger) && lightSum <= currentPlant.Light * (1 + danger))
            {
                if (lightSum > currentPlant.Light)
                {
                    lsdcond = 4;
                }
                else
                {
                    lsdcond = 2;
                }
            }
            else
            {
                if (lightSum > currentPlant.Light)
                {
                    lsdcond = 5;
                }
                else
                {
                    lsdcond = 1;
                }
            }

            // TWILIO!

            // First check if there is something wrong with the plant
            bool isSomethingWrong = false;
            string messageToUser = "";
            string conditionmessage = "";
            if ((smcond == 1 || smcond == 5) || (tcond == 1 || tcond == 5) || (hcond == 1 || hcond == 5) || (lsdcond == 1 || lsdcond == 5))
            {
                messageToUser = "Your " + currentPlant.PlantName + " needs: ";
                isSomethingWrong = true;
                if (smcond == 1)
                {
                    if (conditionmessage == "")
                    {
                        conditionmessage = conditionmessage + "more water";
                    } else
                    {
                        conditionmessage = conditionmessage + ", more water";
                    }
                    
                }
                if (smcond == 5)
                {
                    if (conditionmessage == "")
                    {
                        conditionmessage = conditionmessage + "less water";
                    }
                    else
                    {
                        conditionmessage = conditionmessage + ", less water";
                    }

                }
                if (tcond == 1)
                {
                    if (conditionmessage == "")
                    {
                        conditionmessage = conditionmessage + "warmer temperature";
                    }
                    else
                    {
                        conditionmessage = conditionmessage + ", warmer temperature";
                    }

                }
                if (tcond == 5)
                {
                    if (conditionmessage == "")
                    {
                        conditionmessage = conditionmessage + "cooler temperature";
                    }
                    else
                    {
                        conditionmessage = conditionmessage + ", cooler temperature";
                    }

                }
                if (hcond == 1)
                {
                    if (conditionmessage == "")
                    {
                        conditionmessage = conditionmessage + "more humid environment";
                    }
                    else
                    {
                        conditionmessage = conditionmessage + ", more humid environment";
                    }

                }
                if (hcond == 5)
                {
                    if (conditionmessage == "")
                    {
                        conditionmessage = conditionmessage + "less humid environment";
                    }
                    else
                    {
                        conditionmessage = conditionmessage + ", less humid environment";
                    }

                }
                if (lsdcond == 1)
                {
                    if (conditionmessage == "")
                    {
                        conditionmessage = conditionmessage + "more light";
                    }
                    else
                    {
                        conditionmessage = conditionmessage + ", more light";
                    }

                }
                if (lsdcond == 5)
                {
                    if (conditionmessage == "")
                    {
                        conditionmessage = conditionmessage + "less light";
                    }
                    else
                    {
                        conditionmessage = conditionmessage + ", less light";
                    }

                }
            }

            // Has user enabled Twilio?
            bool isTwilioEnabled = ((from entry in db.AppSettings
                                     where entry.Name == "TwilioEnabled"
                                     select entry).FirstOrDefault().Value == "true");
            string twilioPhoneNumber = (from entry in db.AppSettings
                                        where entry.Name == "TwilioNumber"
                                        select entry).FirstOrDefault().Value;
            if (!isTwilioEnabled)
            {
                isSomethingWrong = false;
            }
            
            if (isSomethingWrong)
            {
                TwilioClient.Init("ACcca828a317b9f85748e680366b1d513f", "64fdbffc4058b616cf7f2a10de5588b8");

                var message = MessageResource.Create(
                    new PhoneNumber("+1" + twilioPhoneNumber),
                    from: new PhoneNumber("+17634529896"),
                    body: messageToUser + conditionmessage
                );
                Console.WriteLine(message.Sid);
            }

            // Create new sensordata object containing complete information to write into db
            SensorData sensorData = new SensorData
            {
                SoilMoisture = SoilMoisture,
                Light = Light,
                Temperature = Temperature,
                Humidity = Humidity,
                CreatedDate = DateTime.Now,
                CareInfoID = currentCareInfoID,
                LightSumDay = lightSum,
                SoilMoistureCondition = smcond,
                LightCondition = lcond,
                TemperatureCondition = tcond,
                HumidityCondition = hcond,
                LightSumDayCondition = lsdcond
            };

            // Need to add Twilio notification functionality here if time permits

            

            db.SensorDatas.Add(sensorData);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = sensorData.SensorDataID }, sensorData);
        }

        // GET: api/SensorDatas/5
        //[ResponseType(typeof(SensorData))]
        //public IHttpActionResult GetSensorData(int id)
        //{
        //    SensorData sensorData = db.SensorDatas.Find(id);
        //    if (sensorData == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(sensorData);
        //}

        // PUT: api/SensorDatas/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutSensorData(int id, SensorData sensorData)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != sensorData.SensorDataID)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(sensorData).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!SensorDataExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        
        // POST: api/SensorDatas
        // DEPRECATED - using GET for db insert action... yes, I know, but firmware team needs this

        //[ResponseType(typeof(SensorData))]
        //public IHttpActionResult PostSensorData(SensorData rawData)
        //{
        //    //float SoilMoisture, float Light, float Temperature, float Humidity
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    // Need to know what the current plant is in order to log it correctly
        //    var currentPlant = (from entry in db.CareDatas
        //                       where entry.Current == true
        //                       select entry).FirstOrDefault();
        //    int currentCareInfoID = currentPlant.CareInfoID;
            
        //    // Create new sensordata object containing complete information to write into db
        //    SensorData sensorData = new SensorData
        //    {
        //        SoilMoisture = rawData.SoilMoisture,
        //        Light = rawData.Light,
        //        Temperature = rawData.Temperature,
        //        Humidity = rawData.Humidity,
        //        CreatedDate = DateTime.Now,
        //        CareInfoID = currentCareInfoID
        //    };

        //    db.SensorDatas.Add(sensorData);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = sensorData.SensorDataID }, sensorData);
        //}

        // DELETE: api/SensorDatas/5
        //[ResponseType(typeof(SensorData))]
        //public IHttpActionResult DeleteSensorData(int id)
        //{
        //    SensorData sensorData = db.SensorDatas.Find(id);
        //    if (sensorData == null)
        //    {
        //        return NotFound();
        //    }

        //    db.SensorDatas.Remove(sensorData);
        //    db.SaveChanges();

        //    return Ok(sensorData);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //private bool SensorDataExists(int id)
        //{
        //    return db.SensorDatas.Count(e => e.SensorDataID == id) > 0;
        //}
    }
}