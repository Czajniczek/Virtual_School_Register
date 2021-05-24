using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Virtual_School_Register.Models
{
    public class File
    {
        public int FileId { get; set; }

        public string Name { get; set; }

        //public string Path { get; set; }

        //public string Description { get; set; }

        //public string Content { get; set; }

        [Display(Name = "Subject")]
        public int SubjectId { get; set; } //Do dodawania (create)

        [Display(Name = "Subject")]
        public virtual Subject Subject { get; set; } //Do wyświetlania (index)
    }
}
