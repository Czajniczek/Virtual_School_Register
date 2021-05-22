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

        [Required]
        [MinLength(1, ErrorMessage = "Content cannot be empty!")]
        public string Content { get; set; }

        [Display(Name = "Answer A")]
        [Required]
        [MinLength(1, ErrorMessage = "Answer cannot be empty!")]
        public string AnswerA { get; set; }

        [Display(Name = "Answer B")]
        [Required]
        [MinLength(1, ErrorMessage = "Answer cannot be empty!")]
        public string AnswerB { get; set; }

        [Display(Name = "Answer C")]
        public string AnswerC { get; set; }

        [Display(Name = "Answer D")]
        public string AnswerD { get; set; }

        [Display(Name = "Correct answer")]
        [Required]
        [MinLength(1, ErrorMessage = "Correct answer cannot be empty!")]
        public string CorrectAnswer { get; set; }

        public int TestId { get; set; }

        public virtual Test Test { get; set; }
    }
}
