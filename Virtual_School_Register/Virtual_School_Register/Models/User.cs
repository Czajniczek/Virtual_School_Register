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
        [StringLength(20, MinimumLength = 1)]
        public string Name { get; set; }

        [StringLength(20, MinimumLength = 1)]
        public string Surname { get; set; }

        [StringLength(1, MinimumLength = 1)]
        public string Sex { get; set; }

        [Display(Name = "Birth date")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [MinLength(1)]
        public string Adress { get; set; }

        [Display(Name = "Parent")]
        public string ParentId { get; set; }

        [Display(Name = "User type")]
        public string Type { get; set; }

        public virtual ICollection<Annoucement> Annoucements { get; set; }

        public virtual ICollection<Message> Messages { get; set; }

        public int? ClassId { get; set; }

        public virtual Class Class { get; set; }

        public virtual ICollection<Evaluation> Evaluations { get; set; }

        public virtual ICollection<ConductingLesson> ConductingLessons { get; set; }
    }
}
