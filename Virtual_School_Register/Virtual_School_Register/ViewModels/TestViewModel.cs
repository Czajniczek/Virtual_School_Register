using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Virtual_School_Register.ViewModels
{
    public class TestViewModel
    {
        public int TestId { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Title cannot be empty!")]
        public string Title { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan Time { get; set; }

        public int ConductingLessonId { get; set; }

        public string SubjectName { get; set; }

        public string ClassName { get; set; }
    }
}
