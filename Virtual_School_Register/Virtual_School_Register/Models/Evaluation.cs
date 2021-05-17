using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Virtual_School_Register.Models
{
    public class Evaluation
    {
        public int EvaluationId { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }

        public int GradeId { get; set; }

        public virtual Grade Grade { get; set; }

        public int SubjectId { get; set; }

        public virtual Subject Subject { get; set; }
    }
}
