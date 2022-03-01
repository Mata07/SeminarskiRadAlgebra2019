using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aplikacija.Models
{
    [Table("Seminari")]
    public class Seminar
    {
        [Key]
        public int SeminarId { get; set; }

        [Required(ErrorMessage = "Naziv seminara je obavezan podatak! Molimo vas unesite naziv seminara.")]
        [StringLength(200, MinimumLength = 2,
            ErrorMessage = "Naziv seminara ne smije imati manje od dva i više od 100 znakova.")]
        public string Naziv { get; set; }

        [Required(ErrorMessage = "Opis seminara je obavezan podatak! Molimo vas unesite opis seminara.")]
        [DataType(DataType.MultilineText)]
        public string Opis { get; set; }

        [Required(ErrorMessage = "Datum početka seminara je obavezan podatak!")]
        [DataType(DataType.Date)]
        [Display(Name = "Datum početka")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Datum { get; set; }

        public bool Popunjen { get; set; }

        public virtual ICollection<Predbiljezba> Predbiljezbe { get; set; }
    }
}