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

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Sex { get; set; }

        public string Email { get; set; }

        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Birth date")]
        public DateTime BirthDate { get; set; }

        public string Adress { get; set; }

        [Display(Name = "Parent")]
        public string ParentId { get; set; }

        [Display(Name = "Class")]
        public int? ClassId { get; set; }

        [Display(Name = "User type")]
        public string Type { get; set; }
    }
}
