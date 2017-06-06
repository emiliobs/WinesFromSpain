using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WinesBackend.Classes;
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
        public async Task<IHttpActionResult> PutWine(int id, WineRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != request.WineId)
            {
                return BadRequest();
            }


            if (request.ImageArray != null && request.ImageArray.Length > 0)
            {
                var stream = new MemoryStream(request.ImageArray);
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                var folder = "~/Content/Images";
                var fullPath = $"{folder}/{file}";
                var response = FilesHelper.UploadPhoto(stream, folder, file);

                if (response)
                {
                    request.Image = fullPath;
                }
            }

            var wine = ToWine(request);

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
        public async Task<IHttpActionResult> PostWine(WineRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            if (request.ImageArray != null && request.ImageArray.Length > 0)
            {
                var stream = new MemoryStream(request.ImageArray);
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                var folder = "~/Content/Images";
                var fullPath = $"{folder}/{file}";
                var response = FilesHelper.UploadPhoto(stream, folder, file);

                if (response)
                {
                    request.Image = fullPath;
                }
            }

            var wine = ToWine(request);

            db.Wines.Add(wine);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = wine.WineId }, wine);
        }

        private Wine ToWine(WineRequest request)
        {
            return  new Wine()
            {
                Name = request.Name,
                Pairing = request.Pairing,
                Type = request.Type,
                Price = request.Price,
                Variety = request.Variety,
                Tasting = request.Tasting,
                Image = request.Image,
                WineId = request.WineId,
                
            };
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