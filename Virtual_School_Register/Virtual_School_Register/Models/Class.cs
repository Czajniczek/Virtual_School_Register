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

        [Required]
        [StringLength(5, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 5 characters!")]
        public string Name { get; set; }

        [Display(Name = "Profile")]
        [Required]
        [MinLength(1, ErrorMessage = "Profile cannot be empty!")]
        public string Content { get; set; }

        [Display(Name = "Class tutor")]
        public string ClassTutorId { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<ConductingLesson> ConductingLessons { get; set; }
    }
}
