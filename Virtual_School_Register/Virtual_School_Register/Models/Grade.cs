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

        [Display(Name = "Ocena")]
        [Required]
        [RegularExpression(@"^\d+.\d{0,1}$", ErrorMessage = "Nieprawidłowa ocena")]
        public float Value { get; set; }

        [Display(Name = "Data")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Display(Name = "Kategoria")]
        [MinLength(1, ErrorMessage = "Kategoria nie może być pusta!")]
        public string Type { get; set; }

        [Display(Name = "Komentarz")]
        public string Comment { get; set; }

        public virtual ICollection<Evaluation> Evaluations { get; set; }
    }
}
