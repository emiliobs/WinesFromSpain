using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WinesBackend.Models;

namespace WinesBackend.Controllers.API
{
    public class WinesController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Wines
        public IQueryable<Wine> GetWines()
        {
            return db.Wines.OrderBy(w=>w.Type).ThenBy(w=>w.Name);
        }

        // GET: api/Wines/5
        [ResponseType(typeof(Wine))]
        public async Task<IHttpActionResult> GetWine(int id)
        {
            Wine wine = await db.Wines.FindAsync(id);
            if (wine == null)
            {
                return NotFound();
            }

            return Ok(wine);
        }

        // PUT: api/Wines/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutWine(int id, Wine wine)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != wine.WineId)
            {
                return BadRequest();
            }

            db.Entry(wine).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WineExists(id))
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

        // POST: api/Wines
        [ResponseType(typeof(Wine))]
        public async Task<IHttpActionResult> PostWine(Wine wine)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Wines.Add(wine);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = wine.WineId }, wine);
        }

        // DELETE: api/Wines/5
        [ResponseType(typeof(Wine))]
        public async Task<IHttpActionResult> DeleteWine(int id)
        {
            Wine wine = await db.Wines.FindAsync(id);
            if (wine == null)
            {
                return NotFound();
            }

            db.Wines.Remove(wine);
            await db.SaveChangesAsync();

            return Ok(wine);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool WineExists(int id)
        {
            return db.Wines.Count(e => e.WineId == id) > 0;
        }
    }
}