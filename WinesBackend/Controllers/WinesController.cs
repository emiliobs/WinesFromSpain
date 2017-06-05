using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WinesBackend.Classes;
using WinesBackend.Models;

namespace WinesBackend.Controllers
{
    public class WinesController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Wines
        public async Task<ActionResult> Index()
        {
            return View(await db.Wines.OrderBy(w=>w.Type).ThenBy(w=>w.Name).ToListAsync());
        }

        // GET: Wines/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Wine wine = await db.Wines.FindAsync(id);

            if (wine == null)
            {
                return HttpNotFound();
            }

            var view = ToView(wine);
           
            return View(view);
        }

        // GET: Wines/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Wines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(WineView view)
        {
            if (ModelState.IsValid)
            {
                var picture = string.Empty;
                var folder = "~/Content/Images";

                if (view.ImageFile != null)
                {
                    picture = FilesHelper.UploadPhoto(view.ImageFile, folder);
                    picture = $"{folder}/{picture}";
                }


                try
                {
                    var wine = ToWine(view);
                    wine.Image = picture;

                    db.Wines.Add(wine);
                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                
            }

            return View(view);
        }

        private Wine ToWine(WineView view)
        {
            return new Wine()
            {
                Name = view.Name,
                Pairing = view.Pairing,
                Type = view.Type,
                Price = view.Price,
                Variety = view.Variety,
                Tasting = view.Tasting,
                WineId = view.WineId,
                Image = view.Image,
            };
        }

        // GET: Wines/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var wine = await db.Wines.FindAsync(id);

            if (wine == null)
            {
                return HttpNotFound();
            }

            var view = ToView(wine);

            return View(view);
        }

        private WineView ToView(Wine wine)
        {
            return  new WineView()
            {
                Name =  wine.Name,
                Pairing = wine.Pairing,
                Type = wine.Type,
                Price = wine.Price,
                Variety = wine.Variety,
                Tasting = wine.Tasting,
                Image = wine.Image,
                WineId = wine.WineId
            };
        }

        // POST: Wines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(WineView view)
        {
            if (ModelState.IsValid)
            {

                var picture = view.Image;
                var folder = "~/Content/Images";

                if (view.ImageFile != null)
                {
                    picture = FilesHelper.UploadPhoto(view.ImageFile, folder);
                    picture = $"{folder}/{picture}";
                }

                try
                {

                    var wine = ToWine(view);
                    wine.Image = picture;

                    db.Entry(wine).State = EntityState.Modified;

                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                
            }
            return View(view);
        }

        // GET: Wines/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Wine wine = await db.Wines.FindAsync(id);

            if (wine == null)
            {
                return HttpNotFound();
            }

            var view = ToView(wine);

            return View(view);
        }

        // POST: Wines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Wine wine = await db.Wines.FindAsync(id);

            db.Wines.Remove(wine);

            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
