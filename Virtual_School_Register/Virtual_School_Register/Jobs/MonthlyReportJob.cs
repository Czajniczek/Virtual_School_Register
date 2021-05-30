using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Virtual_School_Register.Data;
using Virtual_School_Register.EmailConfig;
using Virtual_School_Register.Helpers;
using Virtual_School_Register.Models;
using Virtual_School_Register.ViewModels;

namespace Virtual_School_Register.Jobs
{
    public class MonthlyReportJob : IJob
    {
        private readonly IServiceProvider _provider;

        public MonthlyReportJob(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using (var scope = _provider.GetService<IServiceScopeFactory>().CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var _emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    var students = _context.Users.Where(x => x.Type == "Uczen").ToList();

                    foreach (var student in students)
                    {
                        if (student.ParentId != null)
                        {
                            var subjects = _context.Evaluation
                                .Where(x => x.UserId == student.Id)
                                .Select(x => new SubjectGradeViewModel 
                                { 
                                    SubjectId = x.SubjectId,
                                    SubjectName = x.Subject.Name
                                })
                                .Distinct()
                                .ToList();

                            List<EmailSubjectGrade> emailSubjectGradesList = new List<EmailSubjectGrade>();

                            foreach (var subject in subjects)
                            {
                                ICollection<Evaluation> grades = _context.Evaluation
                                    .Where(x => x.UserId == student.Id && x.SubjectId == subject.SubjectId).ToList();

                                string gradesString = "";

                                foreach (var grade in grades)
                                {
                                    gradesString += grade.Value + ", ";
                                }

                                EmailSubjectGrade emailSubjectGrade = new EmailSubjectGrade();

                                emailSubjectGrade.SubjectName = "<th style='text-align:left'>" + subject.SubjectName + ": " + "</th>";
                                emailSubjectGrade.Grades = "<td>" + gradesString + "</td>";

                                emailSubjectGradesList.Add(emailSubjectGrade);

                                subject.Evaluations = grades;
                            }

                            string datas = "";

                            foreach (var item in emailSubjectGradesList)
                            {
                                //headers += item.SubjectName;
                                datas += "<tr>" + item.SubjectName + item.Grades + "</tr>";
                            }

                            datas += "";

                            var parentEmail = _context.Users.FirstOrDefault(x => x.Id == student.ParentId).Email;

                            var emailMessage = new MyMessage(new string[] { parentEmail }, $"Monthly {student.Name} {student.Surname} grade summary",
                                string.Format($"<table><tbody>{datas}</tbody></table>"));

                            _emailSender.SendEmail(emailMessage);
                        }
                    }

                    transaction.Dispose();
                }
            }

            await Task.CompletedTask;
        }
    }
}
