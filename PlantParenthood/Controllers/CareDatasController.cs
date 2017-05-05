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
    public class CareDatasController : ApiController
    {
        private PlantParenthoodContext db = new PlantParenthoodContext();

        // GET: api/CareDatas
        public IQueryable<CareData> GetCareDatas()
        {
            return db.CareDatas;
        }

        // GET: api/CareDatas/5
        [ResponseType(typeof(CareData))]
        public IHttpActionResult GetCareData(int id)
        {
            CareData careData = db.CareDatas.Find(id);
            if (careData == null)
            {
                return NotFound();
            }

            return Ok(careData);
        }

        // PUT: api/CareDatas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCareData(int id, CareData careData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != careData.CareInfoID)
            {
                return BadRequest();
            }

            db.Entry(careData).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CareDataExists(id))
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

        // POST: api/CareDatas
        [ResponseType(typeof(CareData))]
        public IHttpActionResult PostCareData(CareData careData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CareDatas.Add(careData);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = careData.CareInfoID }, careData);
        }

        // DELETE: api/CareDatas/5
        [ResponseType(typeof(CareData))]
        public IHttpActionResult DeleteCareData(int id)
        {
            CareData careData = db.CareDatas.Find(id);
            if (careData == null)
            {
                return NotFound();
            }

            db.CareDatas.Remove(careData);
            db.SaveChanges();

            return Ok(careData);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CareDataExists(int id)
        {
            return db.CareDatas.Count(e => e.CareInfoID == id) > 0;
        }
    }
}