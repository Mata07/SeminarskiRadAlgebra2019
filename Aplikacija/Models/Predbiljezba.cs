using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplikacija.Models
{
    [Table("Predbiljezbe")]
    public class Predbiljezba
    {
        [Key]
        public int PredbiljezbaId { get; set; }

        [DataType(DataType.Date,
            ErrorMessage = "Datum predbilježbe je obavezan podatak!")]
        [Display(Name = "Datum Predbilježbe")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DatumPredbiljezbe { get; set; }

        [Required(ErrorMessage = "Ime je obavezan podatak! Molimo unesite vaše ime.")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "Ime ne smije biti kraće od 2 ili duže od 100 znakova!")]
        public string Ime { get; set; }

        [Required(ErrorMessage = "Prezime je obavezan podatak! Molimo unesite vaše prezime.")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "Prezime ne smije biti kraće od 2 ili duže od 100 znakova!")]
        public string Prezime { get; set; }       

        [Required(ErrorMessage = "Adresa je obavezan podatak! Molimo unesite vašu adresu.")]
        public string Adresa { get; set; }

        [Required(ErrorMessage = "Email je obavezan podatak! Molimo unesite vaš email.")]
        [StringLength(200, MinimumLength = 2,
            ErrorMessage = "Adresa ne smije biti kraća od 2 ili duža od 200 znakova!")]
        [EmailAddress(ErrorMessage = "Unijeli ste email adresu krivog formata! Molimo unesite ispravan format.(npr: ime@mail.com)")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Broj telefona je obavezan podatak! Molimo unesite vaš broj telefona.")]
        [StringLength(50, MinimumLength = 6,
            ErrorMessage = "Broj telefona ne smije biti kraći od 6 ili duži od 50 znakova!")]
        public string Telefon { get; set; }

        //[UIHint("TemplStatus")] //Uklonio zbog primjene RadioButtonFor u Edit view
        public string Status { get; set; }

        
        public int SeminarId { get; set; }
        public virtual Seminar Seminar { get; set; }


        #region Konstruktori

        public Predbiljezba()
        {
            DatumPredbiljezbe = DateTime.Now;
        } 

        #endregion
    }
}