using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Virtual_School_Register.ViewModels
{
    public class UserDetailsViewModel
    {
        [Display(Name = "ID")]
        public string Id { get; set; }

        [Display(Name = "Login")]
        public string UserName { get; set; }

        [Display(Name = "Imię")]
        public string Name { get; set; }

        [Display(Name = "Nazwisko")]
        public string Surname { get; set; }

        [Display(Name = "Płeć")]
        public string Sex { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Numer telefonu")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Data urodzenia")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Adres")]
        public string Adress { get; set; }

        [Display(Name = "Rodzic")]
        public string ParentId { get; set; }

        [Display(Name = "Klasa")]
        public int? ClassId { get; set; }

        [Display(Name = "Typ użytkownika")]
        public string Type { get; set; }
    }
}
