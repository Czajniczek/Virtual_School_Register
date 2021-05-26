using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Virtual_School_Register.Models
{
    public class Evaluation
    {
        public int EvaluationId { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }

        [Display(Name = "Grade")]
        [Required]
        //[RegularExpression(@"^\d+.\d{0,1}$", ErrorMessage = "Incorrect grade")]
        public string Value { get; set; }

        //[DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Display(Name = "Category")]
        [MinLength(1, ErrorMessage = "Category cannot be empty!")]
        public string Type { get; set; }

        public string Comment { get; set; }

        //public int GradeId { get; set; }

        //public virtual Grade Grade { get; set; }

        public int SubjectId { get; set; }

        public virtual Subject Subject { get; set; }
    }
}
