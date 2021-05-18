using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Virtual_School_Register.Helpers;

namespace Virtual_School_Register.ViewModels
{
    public class CreateUserViewModel
    {
        [Display(Name = "Login")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Imię musi mieć od 1 do 20 znaków!")]
        public string UserName { get; set; }

        [Display(Name = "Imię")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Imię musi mieć od 1 do 20 znaków!")]
        public string Name { get; set; }

        [Display(Name = "Nazwisko")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Nazwisko musi mieć od 1 do 20 znaków!")]
        public string Surname { get; set; }

        [Display(Name = "Płeć")]
        public string Sex { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Numer telefonu")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Data urodzenia")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Adres")]
        [MinLength(1, ErrorMessage = "Opis nie może być pusty!")]
        public string Adress { get; set; }

        [Display(Name = "Rodzic")]
        public string ParentId { get; set; }

        [Display(Name = "Klasa ID")]
        [RequiredIf("Type", "Uczen", "Uczeń musi mieć przypisaną klasę")]
        public int? ClassId { get; set; }

        [Display(Name = "Hasło")]
        [DataType(DataType.Password)]
        [RegularExpression(Consts.PASSWORD_REGEX, ErrorMessage = "Hasło musi zawierać co najmniej 8 znaków, znak specjalny i wielką literę")]
        public string Password { get; set; }

        [Display(Name = "Powtórz hasło")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Hasła nie są identyczne")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Typ użytkownika")]
        public string Type { get; set; }
    }
}
