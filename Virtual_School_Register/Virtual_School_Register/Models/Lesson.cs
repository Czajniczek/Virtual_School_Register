using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Virtual_School_Register.Models
{
    public class Lesson
    {
        public int LessonId { get; set; }

        [Display(Name = "Tytuł")]
        [Required]
        [MinLength(1, ErrorMessage = "Tytuł nie może być pusty!")]
        public string Title { get; set; }

        [Display(Name = "Treść")]
        //[Required]
        //[MinLength(1, ErrorMessage = "Treść nie może być pusta!")]
        public string Content { get; set; }

        public int SubjectId { get; set; }

        public virtual Subject Subject { get; set; }

        public virtual ICollection<File> Files { get; set; }
    }
}
