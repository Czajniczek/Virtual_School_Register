using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Virtual_School_Register.Models;

namespace Virtual_School_Register.ViewModels
{
    public class LessonViewModel
    {
        public int LessonId { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Title cannot be empty!")]
        public string Title { get; set; }

        public string Content { get; set; }

        public int ConductingLessonId { get; set; }

        public string ClassName { get; set; }

        public string SubjectName { get; set; }

        public virtual ConductingLesson ConductingLesson { get; set; }
    }
}