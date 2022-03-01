using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Aplikacija.Models;

namespace Aplikacija.Controllers
{
    //[Authorize]
    [HandleError]
    public class PredbiljezbeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Predbiljezbe
        public ActionResult Index(string searchString, string status)
        {
            List<Predbiljezba> predbiljezbe = new List<Predbiljezba>();

            ViewBag.SearchString = searchString;
            ViewBag.Status = status;
            try
            {
                // Ako postoji vrijednost u searchStringu
                if (!String.IsNullOrEmpty(searchString))
                {                   

                    // Provjera searchStringa i dodatni upiti za filtriranje rezultata po statusu
                    switch (status)
                    {
                        case "Prihvacena":
                            // 1. verzija (nepotrebno korištenje "Prihvacena" kada je to već uvjet za switch case
                            //predbiljezbe = db.Predbiljezbe.Include(p => p.Seminar).Where(p => p.Status.Equals("Prihvacena")
                            //&& p.Seminar.Naziv.Contains(searchString)
                            //|| p.Prezime.Contains(searchString)
                            //|| p.Ime.Contains(searchString)
                            //|| p.Email.Contains(searchString)
                            //|| p.Adresa.Contains(searchString)
                            //|| p.Status.Contains(searchString)
                            //|| p.Telefon.Contains(searchString)).ToList();

                            // 2. bolji pristup (status) umjesto ("Prihvacena")
                            //predbiljezbe = db.Predbiljezbe.Include(p => p.Seminar).Where(p => p.Status.Equals(status)
                            //&& p.Seminar.Naziv.Contains(searchString)
                            //|| p.Prezime.Contains(searchString)
                            //|| p.Ime.Contains(searchString)
                            //|| p.Email.Contains(searchString)
                            //|| p.Adresa.Contains(searchString)
                            //|| p.Status.Contains(searchString)
                            //|| p.Telefon.Contains(searchString)).ToList();

                            // 3. Najbolje - Ovo + metoda Search
                            predbiljezbe = Search(status, searchString);
                            break;
                        case "Odbijena":
                            //predbiljezbe = db.Predbiljezbe.Include(p => p.Seminar).Where(p => p.Status.Equals("Odbijena")
                            //&& p.Seminar.Naziv.Contains(searchString)
                            //|| p.Prezime.Contains(searchString)
                            //|| p.Ime.Contains(searchString)
                            //|| p.Email.Contains(searchString)
                            //|| p.Adresa.Contains(searchString)
                            //|| p.Status.Contains(searchString)
                            //|| p.Telefon.Contains(searchString)).ToList();
                            predbiljezbe = Search(status, searchString);
                            break;
                        case "Neobradena":
                            //predbiljezbe = db.Predbiljezbe.Include(p => p.Seminar).Where(p => p.Status.Equals(null)
                            //&& p.Seminar.Naziv.Contains(searchString)
                            //|| p.Prezime.Contains(searchString)
                            //|| p.Ime.Contains(searchString)
                            //|| p.Email.Contains(searchString)
                            //|| p.Adresa.Contains(searchString)
                            //|| p.Status.Contains(searchString)
                            //|| p.Telefon.Contains(searchString)).ToList();
                            predbiljezbe = Search(status, searchString);
                            break;
                        default:
                            //predbiljezbe = db.Predbiljezbe.Include(p => p.Seminar)
                            //    .Where(p => p.Seminar.Naziv.Contains(searchString)
                            //    || p.Prezime.Contains(searchString)
                            //    || p.Ime.Contains(searchString)
                            //    || p.Email.Contains(searchString)
                            //    || p.Adresa.Contains(searchString)
                            //    || p.Status.Contains(searchString)
                            //    || p.Telefon.Contains(searchString)).ToList();
                            predbiljezbe = Search(status, searchString);
                            break;
                    }
                }
                else
                {
                    // Lista predbilježbi bez searchStringa
                    switch (status)
                    {
                        case "Prihvacena":
                            predbiljezbe = db.Predbiljezbe.Include(p => p.Seminar).Where(p => p.Status.Equals(status)).ToList();
                            break;
                        case "Odbijena":
                            predbiljezbe = db.Predbiljezbe.Include(p => p.Seminar).Where(p => p.Status.Equals("Odbijena")).ToList();
                            break;
                        case "Neobradena":
                            predbiljezbe = db.Predbiljezbe.Include(p => p.Seminar).Where(p => p.Status.Equals(null)).ToList();
                            break;
                        default:
                            predbiljezbe = db.Predbiljezbe.Include(p => p.Seminar).ToList();
                            break;
                    }                    
                }
            }
            catch (SqlException ex)
            {
                ViewBag.Message = string.Format("Dogodila se SQL greška pri izlistanju predbilježbi! Detalji: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                ViewBag.Message = string.Format("Dogodila se greška pri izlistanju predbilježbi! Detalji: {0}",
                    ex.Message);
            }

            return View(predbiljezbe);
        }


        public ActionResult Details(int? id)
        {
            Predbiljezba predbiljezba = null;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                predbiljezba = db.Predbiljezbe.Find(id);
                if (predbiljezba == null)
                {
                    return HttpNotFound();
                }
            }
            catch (SqlException ex)
            {
                ViewBag.Message = string.Format("Dogodila se SQL greška pri dohvaćanju detalja o predbilježbi! Detalji: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                ViewBag.Message = string.Format("Dogodila se greška pri dohvaćanju detalja o predbilježbi! Detalji: {0}", ex.Message);
            }

            return View(predbiljezba);
        }


        [HttpGet]
        public ActionResult Edit(int? id)
        {
            Predbiljezba predbiljezba = null;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                predbiljezba = db.Predbiljezbe.Find(id);
                //predbiljezba.Status = Convert.ToString(predbiljezba.PredbiljezbaId);
                if (predbiljezba == null)
                {
                    return HttpNotFound();
                }

                // Provjerava vrijednost iz db za Predbiljezba.Status
                // i dodaje checked = "checked" ako je true
                // Radi samo sa RadioButtonFor(LINQ expression, value, true/false(checked))                
                //if (predbiljezba.Status == null)
                //{
                //    ViewBag.isChecked = "false";
                //}
                //else if (predbiljezba.Status.Equals("Prihvacena"))
                //{
                //    ViewBag.isChecked = "true";
                //}
                //else if (predbiljezba.Status.Equals("Odbijena"))
                //{
                //    ViewBag.isChecked = "true";
                //}
            }
            catch (SqlException ex)
            {
                ViewBag.Message = string.Format("Dogodila se SQL greška pri uređivanju predbilježbe! Detalji: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                ViewBag.Message = string.Format("Dogodila se greška pri uređivanju predbilježbe! Detalji: {0}", ex.Message);
            }
            
            ViewBag.SeminarId = new SelectList(db.Seminari, "SeminarId", "Naziv", predbiljezba.SeminarId);
            return View(predbiljezba);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PredbiljezbaId,DatumPredbiljezbe,Ime,Prezime,Adresa,Email,Telefon,Status,SeminarId")] Predbiljezba predbiljezba)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(predbiljezba).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["Message"] = "Podaci o predbilježbi su ažurirani!";
                    return RedirectToAction("Index");
                }
                catch (SqlException ex)
                {
                    ViewBag.Message = string.Format("Dogodila se SQL greška pri uređivanju predbilježbe! Detalji: {0}", ex.Message);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = string.Format("Dogodila se greška pri uređivanju predbilježbe! Detalji: {0}", ex.Message);
                }
            }
            ViewBag.SeminarId = new SelectList(db.Seminari, "SeminarId", "Naziv", predbiljezba.SeminarId);
            return View(predbiljezba);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            Predbiljezba predbiljezba = null;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                predbiljezba = db.Predbiljezbe.Find(id);
                if (predbiljezba == null)
                {
                    return HttpNotFound();
                }
            }
            catch (SqlException ex)
            {
                ViewBag.Message = string.Format("Dogodila se SQL greška pri uređivanju predbilježbe! Detalji: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                ViewBag.Message = string.Format("Dogodila se greška pri uređivanju predbilježbe! Detalji: {0}", ex.Message);
            }

            ViewBag.SeminarId = new SelectList(db.Seminari, "SeminarId", "Naziv", predbiljezba.SeminarId);
            return View(predbiljezba);
        }


        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Predbiljezba predbiljezba = null;
            try
            {
                predbiljezba = db.Predbiljezbe.Find(id);
                db.Predbiljezbe.Remove(predbiljezba);
                db.SaveChanges();
                TempData["Message"] = "Predbilježba je obrisana.";
                return RedirectToAction("Index");
            }
            catch (SqlException ex)
            {
                ViewBag.Message = string.Format("Dogodila se SQL greška pri brisanju predbilježbe! Detalji: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                ViewBag.Message = string.Format("Dogodila se greška pri brisanju predbilježbe! Detalji: {0}", ex.Message);
            }
            return View(predbiljezba);
        }
               

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        // Dodatna metoda za Search polje - 3. verzija
        public List<Predbiljezba> Search(string status, string searchString)
        {
            List<Predbiljezba> predbiljezbe = new List<Predbiljezba>();

            predbiljezbe = db.Predbiljezbe.Include(p => p.Seminar).Where(p => p.Status.Equals(status)
                            && p.Seminar.Naziv.Contains(searchString)
                            || p.Prezime.Contains(searchString)
                            || p.Ime.Contains(searchString)
                            || p.Email.Contains(searchString)
                            || p.Adresa.Contains(searchString)
                            || p.Status.Contains(searchString)
                            || p.Telefon.Contains(searchString)).ToList();

            return predbiljezbe;
        }
    }
}
