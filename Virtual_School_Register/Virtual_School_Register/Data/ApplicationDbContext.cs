using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Virtual_School_Register.Models;

namespace Virtual_School_Register.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Virtual_School_Register.Models.Annoucement> Annoucement { get; set; }
        public DbSet<Virtual_School_Register.Models.Class> Class { get; set; }
        public DbSet<Virtual_School_Register.Models.ConductingLesson> ConductingLesson { get; set; }
        public DbSet<Virtual_School_Register.Models.Evaluation> Evaluation { get; set; }
        public DbSet<Virtual_School_Register.Models.File> File { get; set; }
        public DbSet<Virtual_School_Register.Models.Grade> Grade { get; set; }
        public DbSet<Virtual_School_Register.Models.Lesson> Lesson { get; set; }
        public DbSet<Virtual_School_Register.Models.Message> Message { get; set; }
        public DbSet<Virtual_School_Register.Models.Question> Question { get; set; }
        public DbSet<Virtual_School_Register.Models.Subject> Subject { get; set; }
        public DbSet<Virtual_School_Register.Models.Test> Test { get; set; }


    }
}
