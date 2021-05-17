using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Virtual_School_Register.Models
{
    public class Question
    {
        public int QuestionId { get; set; }

        [Display(Name = "Treść")]
        [Required]
        [MinLength(1, ErrorMessage = "Treść nie może być pusta!")]
        public string Content { get; set; }

        [Display(Name = "Odpowiedź A")]
        [Required]
        [MinLength(1, ErrorMessage = "Odpowiedź nie może być pusta!")]
        public string AnswerA { get; set; }

        [Display(Name = "Odpowiedź B")]
        [Required]
        [MinLength(1, ErrorMessage = "Odpowiedź nie może być pusta!")]
        public string AnswerB { get; set; }

        [Display(Name = "Odpowiedź C")]
        public string AnswerC { get; set; }

        [Display(Name = "Odpowiedź D")]
        public string AnswerD { get; set; }

        [Display(Name = "Prawidłowa odpowiedź")]
        [Required]
        [MinLength(1, ErrorMessage = "Prawidłowa odpowiedź nie może być pusta!")]
        public string CorrectAnswer { get; set; }

        public int TestId { get; set; }

        public virtual Test Test { get; set; }
    }
}
