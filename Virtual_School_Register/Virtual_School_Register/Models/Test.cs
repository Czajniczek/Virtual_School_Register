using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Virtual_School_Register.Models
{
    public class Test
    {
        public int TestId { get; set; }

        [Display(Name = "Tytuł")]
        [Required]
        [MinLength(1, ErrorMessage = "Tytuł nie może być pusty!")]
        public string Title { get; set; }

        [Display(Name = "Czas")]
        [DataType(DataType.Time)]
        public DateTime Time { get; set; }

        public int ClassSubjectTeacherId { get; set; }

        public virtual ConductingLesson ConductingLesson { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}
