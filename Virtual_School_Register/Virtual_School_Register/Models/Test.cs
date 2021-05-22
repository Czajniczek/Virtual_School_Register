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

        [Required]
        [MinLength(1, ErrorMessage = "Title cannot be empty!")]
        public string Title { get; set; }

        [DataType(DataType.Time)]
        public DateTime Time { get; set; }

        public int ConductingLessonId { get; set; }

        public virtual ConductingLesson ConductingLesson { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}
