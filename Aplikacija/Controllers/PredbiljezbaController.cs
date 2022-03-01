using Aplikacija.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Aplikacija.Controllers
{
    [HandleError]
    public class PredbiljezbaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Predbiljezba
        public ActionResult Index(string searchString)
        {
            List<Seminar> seminari = new List<Seminar>();

            // Provjera searchStringa i izlistanje samo seminara koji još nisu popunjeni
            try
            {
                if (!String.IsNullOrEmpty(searchString))
                {
                    seminari = db.Seminari.Where(x => x.Naziv.Contains(searchString)
                                                && x.Popunjen.Equals(false)).ToList();
                }
                else
                {
                    seminari = db.Seminari.Where(x => x.Popunjen.Equals(false)).ToList();
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

        //
        [HttpGet]
        public ActionResult Create(int? id)
        {
            Seminar seminar = null;

            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                seminar = db.Seminari.Find(id);
                if (seminar == null)
                {
                    return HttpNotFound();
                }
                ViewBag.NazivSeminara = seminar.Naziv;
            }
            catch (SqlException ex)
            {
                ViewBag.Message = string.Format("Dogodila se SQL greška pri dohvaćanju seminara! Detalji: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                ViewBag.Message = string.Format("Dogodila se greška pri dohvaćanju seminara! Detalji: {0}", ex.Message);
            }

            return View(new Predbiljezba());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PredbiljezbaId,DatumPredbiljezbe,Ime,Prezime,Adresa,Email,Telefon,Status,SeminarId")] Predbiljezba predbiljezba, int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Iz Html.ActionLink uzima route value /id (Seminari.SeminarId)
                    predbiljezba.SeminarId = id;
                    db.Predbiljezbe.Add(predbiljezba);
                    db.SaveChanges();
                    TempData["Message"] = "Uspješno ste se prijavili na seminar.";
                    return RedirectToAction("Index");
                }
                catch (SqlException ex)
                {
                    ViewBag.Message = string.Format("Dogodila se greška SQL Servera pri kreiranju Predbilježbe! Detalji: {0}", ex.Message);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = string.Format("Dogodila se greška pri pri kreiranju Predbilježbe! Detalji: {0}", ex.Message);
                }
            }
            //ViewBag.SeminarId = new SelectList(db.Seminari, "SeminarId", "Naziv");
            return View(predbiljezba);
        }

    }
}