using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Virtual_School_Register.Models
{
    public class Subject
    {
        public int SubjectId { get; set; }

        [Display(Name = "Nazwa")]
        [Required]
        [MinLength(1, ErrorMessage = "Nazwa nie może być pusta!")]
        public string Name { get; set; }

        [Display(Name = "Treść")]
        [Required]
        [MinLength(1, ErrorMessage = "Treść nie może być pusta!")]
        public string Content { get; set; }

        public virtual ICollection<Evaluation> Evaluations { get; set; }

        public virtual ICollection<Lesson> Lessons { get; set; }

        public virtual ICollection<ConductingLesson> ConductingLessons { get; set; }
    }
}
