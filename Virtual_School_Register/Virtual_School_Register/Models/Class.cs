using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Virtual_School_Register.Models
{
    public class Class
    {
        public int ClassId { get; set; }

        [Display(Name = "Nazwa")]
        [Required]
        [StringLength(5, MinimumLength = 1, ErrorMessage = "Nazwa musi mieć od 1 do 5 znaków!")]
        public string Name { get; set; }

        [Display(Name = "Profil")]
        [Required]
        [MinLength(1, ErrorMessage = "Profil nie może być pusty!")]
        public string Content { get; set; }

        [Display(Name = "Wychowawca")]
        public string ClassTutorId { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<ConductingLesson> ConductingLessons { get; set; }
    }
}
