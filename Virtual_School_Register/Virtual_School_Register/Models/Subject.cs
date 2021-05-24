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

        [Required]
        [MinLength(1, ErrorMessage = "Name cannot be empty!")]
        public string Name { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Content cannot be empty!")]
        public string Content { get; set; }

        public virtual ICollection<Evaluation> Evaluations { get; set; }

        //public virtual ICollection<Lesson> Lessons { get; set; }

        public virtual ICollection<ConductingLesson> ConductingLessons { get; set; }

        public virtual ICollection<File> Files { get; set; }
    }
}
