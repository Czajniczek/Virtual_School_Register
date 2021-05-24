using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Virtual_School_Register.Models
{
    public class ConductingLesson
    {
        public int ConductingLessonId { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }

        public int ClassId { get; set; }

        public virtual Class Class { get; set; }

        public int SubjectId { get; set; }

        public virtual Subject Subject { get; set; }

        public virtual ICollection<Test> Tests { get; set; }

        public virtual ICollection<Lesson> Lessons { get; set; }
    }
}
