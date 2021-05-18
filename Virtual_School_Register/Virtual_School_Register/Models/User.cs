using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Virtual_School_Register.Models
{
    public class User : IdentityUser
    {
        [Display(Name = "Imię")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Imię musi mieć od 1 do 20 znaków!")]
        public string Name { get; set; }

        [Display(Name = "Nazwisko")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Nazwisko musi mieć od 1 do 20 znaków!")]
        public string Surname { get; set; }

        [Display(Name = "Płeć")]
        [StringLength(1, MinimumLength = 1, ErrorMessage = "Płeć musi składać się z jednego znaku")]
        public string Sex { get; set; }

        [Display(Name = "Data urodzenia")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Adres")]
        [MinLength(1, ErrorMessage = "Opis nie może być pusty!")]
        public string Adress { get; set; }

        [Display(Name = "Rodzic")]
        public string ParentId { get; set; }

        //[Display(Name = "Typ użytkownika")]
        //[Required]
        //public string Type { get; set; }

        public virtual ICollection<Annoucement> Annoucements { get; set; }

        public virtual ICollection<Message> Messages { get; set; }

        [Display(Name = "Klasa ID")]
        public int? ClassId { get; set; }

        [Display(Name = "Klasa")]
        public virtual Class Class { get; set; }

        public virtual ICollection<Evaluation> Evaluations { get; set; }

        public virtual ICollection<ConductingLesson> ConductingLessons { get; set; }
    }
}
