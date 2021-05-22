using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Virtual_School_Register.ViewModels
{
    public class UserEditViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Login")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Login must be between 1 and 20 characters!")]
        public string UserName { get; set; }

        [StringLength(20, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 20 characters!")]
        public string Name { get; set; }

        [StringLength(20, MinimumLength = 1, ErrorMessage = "Surname must be between 1 and 20 characters!")]
        public string Surname { get; set; }

        public string Sex { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Phone number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Birth date")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [MinLength(1, ErrorMessage = "Adress cannot be empty!")]
        public string Adress { get; set; }

        [Display(Name = "Parent")]
        public string ParentId { get; set; }

        [Display(Name = "Class")]
        //[RequiredIf("Type", "Uczen", "Student must be assigned to the class")]
        public int? ClassId { get; set; }
    }
}
