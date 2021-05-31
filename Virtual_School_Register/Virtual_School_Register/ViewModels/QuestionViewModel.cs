using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Virtual_School_Register.ViewModels
{
    public class QuestionViewModel
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

        public string Answer { get; set; }

        public int MyPoints { get; set; }

        public int MyQuestion { get; set; }

        [Display(Name = "Correct answer")]
        [Required]
        [MinLength(1, ErrorMessage = "Correct answer cannot be empty!")]
        public string CorrectAnswer { get; set; }

        public int Points { get; set; }

        public int TestId { get; set; }
    }
}
