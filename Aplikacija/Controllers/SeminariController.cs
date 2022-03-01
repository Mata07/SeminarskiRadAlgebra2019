using Aplikacija.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Aplikacija.Controllers
{
    //[Authorize]
    [HandleError]
    public class SeminariController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Seminari
        public ActionResult Index(string searchString)
        {
            List<Seminar> seminari = new List<Seminar>();

            try
            {
                if (!String.IsNullOrEmpty(searchString))
                {
                    seminari = db.Seminari.Where(x => x.Naziv.Contains(searchString)).ToList();
                }
                else
                {
                    seminari = db.Seminari.ToList();
                }
            }
            catch (SqlException ex)
            {
                ViewBag.Message = string.Format("Dogodila se SQL greška pri izlistanju seminara! Detalji: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                ViewBag.Message = string.Format("Dogodila se greška pri izlistanju seminara! Detalji: {0}",
                    ex.Message);
            }
            return View(seminari);
        }

        public ActionResult Details(int? id)
        {
            Seminar seminar = null;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                seminar = db.Seminari.Find(id);
                if (seminar == null)
                {
                    return HttpNotFound();
                }
            }
            catch (SqlException ex)
            {
                ViewBag.Message = string.Format("Dogodila se SQL greška pri dohvaćanju detalja o seminaru! Detalji: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                ViewBag.Message = string.Format("Dogodila se greška pri dohvaćanju detalja o seminaru! Detalji: {0}", ex.Message);
            }
            return View(seminar);
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View(new Seminar());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SeminarId,Naziv,Opis,Datum,Popunjen")]Seminar seminar)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Seminari.Add(seminar);
                    db.SaveChanges();
                    TempData["Message"] = "Novi seminar je unešen!";
                    return RedirectToAction("Index");
                }
                catch (SqlException ex)
                {
                    ViewBag.Message = string.Format("Dogodila se SQL greška pri spremanju seminara! Detalji: {0}", ex.Message);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = string.Format("Dogodila se greška pri spremanju seminara! Detalji: {0}", ex.Message);
                }
            }
            return View(seminar);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            Seminar seminar = null;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                seminar = db.Seminari.Find(id);
                if (seminar == null)
                {
                    return HttpNotFound();
                }
            }
            catch (SqlException ex)
            {
                ViewBag.Message = string.Format("Dogodila se SQL greška pri dohvaćanju seminara! Detalji: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                ViewBag.Message = string.Format("Dogodila se greška pri dohvaćanju seminara! Detalji: {0}", ex.Message);
            }
            return View(seminar);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SeminarId,Naziv,Opis,Datum,Popunjen")]Seminar seminar)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(seminar).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["Message"] = "Podaci o seminaru su ažurirani.";
                    return RedirectToAction("Index");
                }
                catch (SqlException ex)
                {
                    ViewBag.Message = string.Format("Dogodila se SQL greška pri uređivanju seminara! Detalji: {0}", ex.Message);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = string.Format("Dogodila se greška pri uređivanju seminara! Detalji: {0}", ex.Message);
                }
            }
            return View(seminar);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            Seminar seminar = null;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                seminar = db.Seminari.Find(id);
                if (seminar == null)
                {
                    return HttpNotFound();
                }
            }
            catch (SqlException ex)
            {
                ViewBag.Message = string.Format("Dogodila se SQL greška pri dohvaćanju detalja seminara! Detalji: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                ViewBag.Message = string.Format("Dogodila se greška pri dohvaćanju detalja seminara! Detalji: {0}", ex.Message);
            }

            return View(seminar);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Seminar seminar = null;
            try
            {
                seminar = db.Seminari.Find(id);
                db.Seminari.Remove(seminar);
                db.SaveChanges();
                TempData["Message"] = "Seminar je obrisan.";
                return RedirectToAction("Index");
            }
            catch (SqlException ex)
            {
                ViewBag.Message = string.Format("Dogodila se SQL greška pri brisanju seminara! Detalji: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                ViewBag.Message = string.Format("Dogodila se greška pri brisanju seminara! Detalji: {0}", ex.Message);
            }
            return View(seminar);
        }

        // Dohvaća broj prihvaćenih predbilježbi za pojedini seminar
        // ChildActionOnly - ne može se pristupiti preko rute(url) nego samo iz View-a
        [ChildActionOnly]
        public int BrojPolaznika(int id)
        {
            int count = db.Predbiljezbe.Where(x => x.Status.Contains("Prihvacen")
                                                && x.SeminarId.Equals(id)).Count();
            return count;
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