using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Virtual_School_Register.Models
{
    public class Grade
    {
        public int GradeId { get; set; }

        [Display(Name = "Grade")]
        [Required]
        [RegularExpression(@"^\d+.\d{0,1}$", ErrorMessage = "Incorrect grade")]
        public float Value { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Display(Name = "Category")]
        [MinLength(1, ErrorMessage = "Category cannot be empty!")]
        public string Type { get; set; }

        public string Comment { get; set; }

        //public virtual ICollection<Evaluation> Evaluations { get; set; }
    }
}
