using Microsoft.AspNet.Identity.EntityFramework;
using Projekat.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Projekat.Infrastructure
{
   
    public class AuthContext : IdentityDbContext<IdentityUser>
    {
        public AuthContext() : base("ProjectConnection") {
           
            Database.SetInitializer<AuthContext>(new DataAccessContextInitializer());
      }

        public DbSet<Class> Classes { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<ApplicationUser> AppUsers { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<SubjectTeacher> SubjectTeachers { get; set; }
        public DbSet<ClassSubjectTeacher> ClassSubjectTeachers { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Admin>().ToTable("Admin");
            modelBuilder.Entity<Teacher>().ToTable("Teacher");
            modelBuilder.Entity<Parent>().ToTable("Parent");
            modelBuilder.Entity<Student>().ToTable("Student");

           

            modelBuilder.Entity<Student>()
                .HasOptional(student => student.Class)
                .WithMany(Class => Class.Students);

          
            modelBuilder.Entity<Student>()
                .HasOptional(student => student.Parent)
                .WithMany(parent => parent.Children);

            modelBuilder.Entity<SubjectTeacher>()
                .HasOptional(subjectTeacher => subjectTeacher.Subject)
                .WithMany(subject => subject.SubjectTeachers);


            modelBuilder.Entity<SubjectTeacher>()
              .HasOptional(subjectTeacher => subjectTeacher.Teacher)
              .WithMany(teacher => teacher.TaughtSubjects);

            modelBuilder.Entity<ClassSubjectTeacher>()
                .HasOptional(classSubjectTeacher => classSubjectTeacher.SubjectTeacher)
                .WithMany(clas => clas.TaughtSubjectClasses);

            modelBuilder.Entity<ClassSubjectTeacher>()
               .HasOptional(classSubjectTeacher => classSubjectTeacher.Class)
               .WithMany(subjectTeacher => subjectTeacher.AttendedTeacherSubjects);


            modelBuilder.Entity<Grade>()
                         .HasRequired(grade => grade.Student)
                         .WithMany(student => student.Grades);

            modelBuilder.Entity<Grade>()
                       .HasRequired(grade => grade.ClassSubjectTeacher)
                       .WithMany(classSubjectTeacher => classSubjectTeacher.Grades)
                       .WillCascadeOnDelete(false);

           
        }

        
    }
}