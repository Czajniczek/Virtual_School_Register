using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Virtual_School_Register.Models;
using Virtual_School_Register.ViewModels;

namespace Virtual_School_Register.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Annoucement> Annoucement { get; set; }
        public DbSet<Class> Class { get; set; }
        public DbSet<ConductingLesson> ConductingLesson { get; set; }
        public DbSet<Evaluation> Evaluation { get; set; }
        public DbSet<File> File { get; set; }
        public DbSet<Grade> Grade { get; set; }
        public DbSet<Lesson> Lesson { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<Subject> Subject { get; set; }
        public DbSet<Test> Test { get; set; }
        public DbSet<Virtual_School_Register.ViewModels.UserDetailsViewModel> UserDetailsViewModel { get; set; }
    }
}
