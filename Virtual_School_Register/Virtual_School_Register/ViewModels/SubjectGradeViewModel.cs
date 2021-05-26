using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Virtual_School_Register.Models;

namespace Virtual_School_Register.ViewModels
{
    public class SubjectGradeViewModel
    {
        public int SubjectId { get; set; }

        public string SubjectName { get; set; }

        public virtual ICollection<Evaluation> Evaluations { get; set; }
    }
}
