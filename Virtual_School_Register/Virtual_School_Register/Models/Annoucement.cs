using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Virtual_School_Register.Models
{
    public class Annoucement
    {
        public int AnnoucementId { get; set; }

        [Display(Name = "Tytuł")]
        [Required]
        [MinLength(1, ErrorMessage = "Tytuł nie może być pusty!")]
        public string Title { get; set; }

        [Display(Name = "Treść")]
        [Required]
        [MinLength(1, ErrorMessage = "Treść nie może być pusta!")]
        public string Content { get; set; }

        [Display(Name = "Data")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }
    }
}
