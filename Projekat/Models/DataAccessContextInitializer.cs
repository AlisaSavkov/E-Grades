using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Projekat.Infrastructure;
using Projekat.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class DataAccessContextInitializer : DropCreateDatabaseAlways<AuthContext>
    {

        protected override void Seed(AuthContext context)
        {
           
            var store = new RoleStore<IdentityRole>(context);
            var manager = new RoleManager<IdentityRole>(store);

            manager.Create(new IdentityRole("admins"));
            manager.Create(new IdentityRole("students"));
            manager.Create(new IdentityRole("parents"));
            manager.Create(new IdentityRole("teachers"));

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);


            IList<Class> classes = new List<Class>();
            Class class1 = new Class()
            {
                Label = "a",
                Year = 1

            };
            classes.Add(class1);

            Class class2 = new Class()
            {
                Label = "b",
                Year = 2

            };
            classes.Add(class2);

            Class class3 = new Class()
            {
                Label = "b",
                Year = 8

            };
            classes.Add(class3);

            Class class4 = new Class()
            {
                Label = "d",
                Year = 5

            };

            classes.Add(class4);


            Class class5 = new Class()
            {
                Label = "d",
                Year = 2

            };

            classes.Add(class5);

            Class class6 = new Class()
            {
                Label = "c",
                Year = 8

            };

            classes.Add(class6);
            context.Classes.AddRange(classes);

            IList<Subject> subjects = new List<Subject>();
            Subject s1 = new Subject()
            {
                Name = "Matematika",
                LessonNumber = 4,
                Year = 1
            };
            subjects.Add(s1);

            Subject s2 = new Subject()
            {
                Name = "Matematika",
                LessonNumber = 4,
                Year = 2
            };
            subjects.Add(s2);

            Subject s3 = new Subject()
            {
                Name = "Engleski jezik",
                LessonNumber = 4,
                Year = 8
            };
            subjects.Add(s3);

            Subject s4 = new Subject()
            {
                Name = "Srpski",
                LessonNumber = 4,
                Year = 7
            };
            subjects.Add(s4);
            Subject s5 = new Subject()
            {
                Name = "Srpski jezik",
                LessonNumber = 4,
                Year = 1
            };
            subjects.Add(s5);


            //Subject s6 = new Subject()
            //{
            //    Name = "Matematika",
            //    LessonNumber = 5,
            //    Year = 8
            //};
            Subject s6 = new Subject()
            {
                Name = "Priroda i drustvo",
                LessonNumber = 5,
                Year = 2
            };
            subjects.Add(s6);
            Subject s7 = new Subject()
            {
                Name = "Likovna kultura",
                LessonNumber = 2,
                Year = 1
            };
            subjects.Add(s7);

            Subject s8 = new Subject()
            {
                Name = "Muzicka kultura",
                LessonNumber = 4,
                Year = 1
            };
            subjects.Add(s8);
            context.Subjects.AddRange(subjects);

          

            Admin a1 = new Admin();
            a1.FirstName = "Bora";
            a1.LastName = "Boric";
            a1.UserName = "Bora1";
            a1.ShortName = "Bora";
            a1.Email = "bora77@gmali.com";
            userManager.Create(a1, "password123");
            userManager.AddToRole(a1.Id, "admins");

            Admin a2 = new Admin();
            a2.FirstName = "Mira";
            a2.LastName = "Boric";
            a2.UserName = "Mira1";
            a2.ShortName = "Mira";
            a2.Email = "mira43@gmali.com";
            userManager.Create(a2, "password123");
            userManager.AddToRole(a2.Id, "admins");

            Admin a3 = new Admin();
            a3.FirstName = "Jelena";
            a3.LastName = "Dobranic";
            a3.UserName = "Jela1";
            a3.ShortName = "Jela";
            a3.Email = "jelena90@gmali.com";
            userManager.Create(a3, "password123");
            userManager.AddToRole(a3.Id, "admins");

            Admin a4 = new Admin();
            a4.FirstName = "Marko";
            a4.LastName = "Bosic";
            a4.UserName = "Marko5";
            a4.ShortName = "Marko";
            a4.Email = "marko00@gmali.com";
            userManager.Create(a4, "password123");
            userManager.AddToRole(a4.Id, "admins");

            Student st1 = new Student();
            st1.FirstName = "Tina";
            st1.LastName = "Milic";
            st1.UserName = "Milana12";
            //st1.Class = class1;
            class1.Students.Add(st1);
            st1.DateOfBirth= new DateTime(2005, 3, 13);
            st1.ImagePath = "slika4.png";
            st1.Email = "tina009@hotmail.com";
            //students.Add(st1);
            userManager.Create(st1, "pass1297");
            userManager.AddToRole(st1.Id, "students");

            Student st2 = new Student();
            st2.FirstName = "Bojan";
            st2.LastName = "Bozic";
            st2.UserName = "Bojan3";
            st2.Email = "bojan987@hotmail.com";
            //st2.Class = class3;
            class3.Students.Add(st2);
            st2.DateOfBirth = new DateTime(2010, 2, 24);
            st2.ImagePath = "slika2.png";
            userManager.Create(st2, "pass12978");
            userManager.AddToRole(st2.Id, "students");


            Student st3 = new Student();
            st3.FirstName = "Milica";
            st3.LastName = "Milic";
            st3.UserName = "Milica2";
            st3.DateOfBirth = new DateTime(2011, 10, 21);
            st3.Email = "milica453@hotmail.com";
            st3.ImagePath = "slika3.jpeg";
            //st3.Class = class3;
            class5.Students.Add(st3);
            userManager.Create(st3, "pass12978");
            userManager.AddToRole(st3.Id, "students");

            Student st4 = new Student();
            st4.FirstName = "Jelena";
            st4.LastName = "Bogunovic";
            st4.UserName = "jelena6";
            st4.Class = class1;
            st4.Email = "jelena432@hotmail.com";
            st4.DateOfBirth = new DateTime(2011, 9, 21);
           
            class1.Students.Add(st4);
            userManager.Create(st4, "pass12978");
            userManager.AddToRole(st4.Id, "students");


            Student st5 = new Student();
            st5.FirstName = "Boris";
            st5.LastName = "Galic";
            st5.UserName = "boris5";
            st5.DateOfBirth = new DateTime(2009, 11, 25);
            st5.Email = "boris23@hotmail.com";
            //st5.Class = class3;
            class3.Students.Add(st5);
            st5.ImagePath="slika1.png";
            userManager.Create(st5, "pass12978");
            userManager.AddToRole(st5.Id, "students");

            Student st6 = new Student();
            st6.FirstName = "Boris";
            st6.LastName = "Selic";
            st6.UserName = "boris7";
            //st6.Class = class4;
            st6.Email = "boris234@hotmail.com";
            st6.DateOfBirth = new DateTime(2008, 3, 9);
            class4.Students.Add(st6);
            userManager.Create(st6, "pass12978");
            userManager.AddToRole(st6.Id, "students");

            Student st7 = new Student();
            st7.FirstName = "Mirka";
            st7.LastName = "Bogunovic";
            st7.UserName = "mirka09";
            st7.DateOfBirth = new DateTime(2008, 12, 19);
            st7.Email = "mirka125@hotmail.com";
            //st6.Class = class4;
            class4.Students.Add(st7);
            userManager.Create(st7, "pass12978");
            userManager.AddToRole(st7.Id, "students");

            Student st8= new Student();
            st8.FirstName = "Milica";
            st8.LastName = "Bozic";
            st8.UserName = "mica0001";
            st8.DateOfBirth = new DateTime(2009, 12, 10);
            st8.Email = "milca0009@hotmail.com";
            //st6.Class = class4;
            class1.Students.Add(st8);
            userManager.Create(st8, "pass12978");
            userManager.AddToRole(st8.Id, "students");


            Student st9 = new Student();
            st9.FirstName = "Dragana";
            st9.LastName = "Bozic";
            st9.UserName = "dragana12";
            st9.DateOfBirth = new DateTime(2008, 7, 23);
            st9.Email = "draganaj76@hotmail.com";
            //st6.Class = class4;
            class6.Students.Add(st9);
            userManager.Create(st9, "pass12978");
            userManager.AddToRole(st9.Id, "students");

            Student st10 = new Student();
            st10.FirstName = "Milana";
            st10.LastName = "Ilic";
            st10.UserName = "Milana9";
            st10.DateOfBirth = new DateTime(2008, 7, 23);
            st10.Email = "milana90@hotmail.com";
            //st6.Class = class4;
            class6.Students.Add(st10);
            userManager.Create(st10, "pass12978");
            userManager.AddToRole(st10.Id, "students");

            Student st11 = new Student();
            st11.FirstName = "Nenad";
            st11.LastName = "Nenadovic";
            st11.UserName = "Nenad2";
            st11.DateOfBirth = new DateTime(2008, 7, 23);
            st11.Email = "nenad45@hotmail.com";
            //st6.Class = class4;
            class6.Students.Add(st11);
            userManager.Create(st11, "pass12978");
            userManager.AddToRole(st11.Id, "students");


            Student st12 = new Student();
            st12.FirstName = "Sanela";
            st12.LastName = "Milic";
            st12.UserName = "Sanel8";
            st12.DateOfBirth = new DateTime(2008, 8, 12);
            st12.Email = "nenad45@hotmail.com";
            //st6.Class = class4;
            class3.Students.Add(st12);
            userManager.Create(st12, "pass12978");
            userManager.AddToRole(st12.Id, "students");

            Student st13 = new Student();
            st13.FirstName = "Helena";
            st13.LastName = "Sanic";
            st13.UserName = "Helena";
            st13.DateOfBirth = new DateTime(2008,1, 12);
            st13.Email = "nenad45@hotmail.com";
            //st6.Class = class4;
            class3.Students.Add(st13);
            userManager.Create(st13, "pass12978");
            userManager.AddToRole(st13.Id, "students");

            Parent p1 = new Parent();
            p1.FirstName = "Dragana";
            p1.LastName = "Milic";
            p1.UserName = "Dragana1";
            p1.Email = "dragana1@gmail.com";
            p1.JMBG = "1291876203980";
            p1.Children.Add(st1);
            p1.Children.Add(st12);
            p1.Children.Add(st3);
            userManager.Create(p1, "p1239je");
            userManager.AddToRole(p1.Id, "parents");

            Parent p2 = new Parent();
            p2.FirstName = "Mila";
            p2.LastName = "Bozic";
            p2.Email = "mila87@gmail.com";
            p2.UserName = "Mila3";
            p2.JMBG = "1291806203983";
            p2.Children.Add(st2);
            p2.Children.Add(st8);
            p2.Children.Add(st9);
            userManager.Create(p2, "p1239je");
            userManager.AddToRole(p2.Id, "parents");

            Parent p3 = new Parent();
            p3.FirstName = "Jelica";
            p3.LastName = "Bogunovic";
            p3.Email = "jelica45@gmail.com";
            p3.UserName = "Jela4";
            p3.JMBG = "2200806203982";
            p3.Children.Add(st4);
            p3.Children.Add(st7);
            userManager.Create(p3, "p1239je");
            userManager.AddToRole(p3.Id, "parents");

            Parent p4 = new Parent();
            p4.FirstName = "Marija";
            p4.LastName = "Galic";
            p4.Email = "alisa.savkov@gmail.com";
            p4.UserName = "Marija1";
            p4.JMBG = "2200806201983";
            p4.Children.Add(st5);
            
            p4.Children.Add(st10);
            userManager.Create(p4, "p1239je");
            userManager.AddToRole(p4.Id, "parents");

            Parent p5 = new Parent();
            p5.FirstName = "Milica";
            p5.LastName = "Selic";
            p5.Email = "alisa.savkov@gmail.com";
            p5.UserName = "Milica";
            p5.JMBG = "1200806202981";
            //p5.Children.Add(st4);
            p5.Children.Add(st6);
            //p5.Children.Add(st7);
            userManager.Create(p5, "p1239je");
            userManager.AddToRole(p5.Id, "parents");

            Parent p6 = new Parent();
            p6.FirstName = "Ivana";
            p6.LastName = "Nenadovic";
            p6.Email = "alisa.savkov@gmail.com";
            p6.UserName = "Iva1";
            p6.JMBG = "0200236202981";
            p6.Children.Add(st11);
            p6.Children.Add(st13);
            userManager.Create(p6, "p1239je");
            userManager.AddToRole(p6.Id, "parents");


            Teacher t1 = new Teacher();
            t1.Email = "radoslav@gmail.com";
            t1.UserName = "Radoslav1";
            t1.FirstName = "Radoslav";
            t1.LastName = "Bozic";
            t1.JMBG = "0923764532781";
            userManager.Create(t1, "pass1297");
            userManager.AddToRole(t1.Id, "teachers");
           // teachers.Add(t1);

            Teacher t2 = new Teacher();
            t2.Email = "mirko@gmail.com";
            t2.UserName = "Mirko3";
            t2.FirstName = "Mirko";
            t2.LastName = "Miric";
            t2.JMBG = "0987623456723";
            userManager.Create(t2, "pass1297");
            userManager.AddToRole(t2.Id, "teachers");
           // teachers.Add(t2);

            Teacher t3 = new Teacher();
            t3.Email = "jovan1@gmail.com";
            t3.UserName = "Jovan1";
            t3.FirstName = "Jovan";
            t3.LastName = "Jovic";
            t3.JMBG = "9873456278109";
            userManager.Create(t3, "pass1297");
            userManager.AddToRole(t3.Id, "teachers");

            Teacher t4 = new Teacher();
            t4.Email = "ivana771@gmail.com";
            t4.UserName = "Ivana5";
            t4.FirstName = "Ivana";
            t4.LastName = "Ivic";
            t4.JMBG = "1177898765429";
            userManager.Create(t4, "pass1297");
            userManager.AddToRole(t4.Id, "teachers");

            Teacher t5 = new Teacher();
            t5.Email = "dragica45@gmail.com";
            t5.UserName = "Dragica3";
            t5.FirstName = "Dragica";
            t5.LastName = "Bozic";
            t5.JMBG = "1670987543765";
            userManager.Create(t5, "pass1297");
            userManager.AddToRole(t5.Id, "teachers");


            Teacher t6 = new Teacher();
            t6.Email = "boki9@gmail.com";
            t6.UserName = "Bojan1";
            t6.FirstName = "Bojan";
            t6.LastName = "Dragic";
            t6.JMBG = "1670987003765";
            userManager.Create(t6, "pass1297");
            userManager.AddToRole(t6.Id, "teachers");

            Teacher t7 = new Teacher();
            t7.Email = "rade009@gmail.com";
            t7.UserName = "Rade";
            t7.FirstName = "Radomir";
            t7.LastName = "Radic";
            t7.JMBG = "1570987003765";
            userManager.Create(t7, "pass1297");
            userManager.AddToRole(t7.Id, "teachers");

            Teacher t8 = new Teacher();
            t8.Email = "milena43@gmail.com";
            t8.UserName = "Mila";
            t8.FirstName = "Milena";
            t8.LastName = "Jankovic";
            t8.JMBG = "1570988003765";
            userManager.Create(t8, "pass1297");
            userManager.AddToRole(t8.Id, "teachers");

            Teacher t9 = new Teacher();
            t9.Email = "silvana99@gmail.com";
            t9.UserName = "Silvana9";
            t9.FirstName = "Silvana";
            t9.LastName = "Maric";
            t9.JMBG = "1502988003764";
            userManager.Create(t9, "pass1297");
            userManager.AddToRole(t9.Id, "teachers");

            Teacher t10 = new Teacher();
            t10.Email = "hana77@gmail.com";
            t10.UserName = "Hana";
            t10.FirstName = "Hana";
            t10.LastName = "Ivancic";
            t10.JMBG = "1508988003765";
            userManager.Create(t10, "pass1297");
            userManager.AddToRole(t10.Id, "teachers");

            IList<SubjectTeacher> subjectTeachers = new List<SubjectTeacher>();

            SubjectTeacher subteach1 = new SubjectTeacher();
            subteach1.Subject = s1;
            subteach1.Teacher = t1;
            s1.SubjectTeachers.Add(subteach1);
            t1.TaughtSubjects.Add(subteach1);
            subjectTeachers.Add(subteach1);

            SubjectTeacher subteach2 = new SubjectTeacher();
            subteach2.Subject = s1;
            subteach2.Teacher = t2;
            s1.SubjectTeachers.Add(subteach2);
            t2.TaughtSubjects.Add(subteach2);
            subjectTeachers.Add(subteach2);


            SubjectTeacher subteach3 = new SubjectTeacher();
            subteach3.Subject = s3;
            subteach3.Teacher = t2;
            s3.SubjectTeachers.Add(subteach3);
            t2.TaughtSubjects.Add(subteach3);
            subjectTeachers.Add(subteach3);

            SubjectTeacher subteach4 = new SubjectTeacher();
            subteach4.Subject = s3;
            subteach4.Teacher = t3;
            s3.SubjectTeachers.Add(subteach4);
            t3.TaughtSubjects.Add(subteach4);
            subjectTeachers.Add(subteach4);


            SubjectTeacher subteach6 = new SubjectTeacher();
            subteach6.Subject = s3;
            subteach6.Teacher = t4;
            s3.SubjectTeachers.Add(subteach6);
            t4.TaughtSubjects.Add(subteach6);
            subjectTeachers.Add(subteach6);


            SubjectTeacher subteach7 = new SubjectTeacher();
            subteach7.Subject = s7;
            subteach7.Teacher = t3;
            s7.SubjectTeachers.Add(subteach7);
            t3.TaughtSubjects.Add(subteach7);
            subjectTeachers.Add(subteach7);
            
            SubjectTeacher subteach8 = new SubjectTeacher();
            subteach8.Subject = s5;
            subteach8.Teacher = t3;
            s5.SubjectTeachers.Add(subteach8);
            t3.TaughtSubjects.Add(subteach8);
            subjectTeachers.Add(subteach8);

            SubjectTeacher subteach9 = new SubjectTeacher();
            subteach9.Subject = s6;
            subteach9.Teacher = t1;
            s6.SubjectTeachers.Add(subteach9);
            t1.TaughtSubjects.Add(subteach9);
            subjectTeachers.Add(subteach9);

            SubjectTeacher subteach11 = new SubjectTeacher();
            subteach11.Subject = s2;
            subteach11.Teacher = t1;
            s2.SubjectTeachers.Add(subteach11);
            t1.TaughtSubjects.Add(subteach11);
            subjectTeachers.Add(subteach11);

            SubjectTeacher subteach12 = new SubjectTeacher();
            subteach12.Subject = s4;
            subteach12.Teacher = t10;
            s4.SubjectTeachers.Add(subteach12);
            t10.TaughtSubjects.Add(subteach12);
            subjectTeachers.Add(subteach12);


            SubjectTeacher subteach13 = new SubjectTeacher();
            subteach13.Subject = s8;
            subteach13.Teacher = t10;
            s8.SubjectTeachers.Add(subteach13);
            t10.TaughtSubjects.Add(subteach13);
            subjectTeachers.Add(subteach13);
            context.SubjectTeachers.AddRange(subjectTeachers);

            IList<ClassSubjectTeacher> csts = new List<ClassSubjectTeacher>();
            ClassSubjectTeacher cst1 = new ClassSubjectTeacher();
            cst1.Class = class1;
            class1.AttendedTeacherSubjects.Add(cst1);
            cst1.SubjectTeacher = subteach1;
            subteach1.TaughtSubjectClasses.Add(cst1);
            csts.Add(cst1);


            ClassSubjectTeacher cst3 = new ClassSubjectTeacher();
            cst3.Class = class3;
            class3.AttendedTeacherSubjects.Add(cst3);
            cst3.SubjectTeacher = subteach4;
            subteach4.TaughtSubjectClasses.Add(cst3);
            csts.Add(cst3);

            ClassSubjectTeacher cst4 = new ClassSubjectTeacher();
            cst4.Class = class6;
            class6.AttendedTeacherSubjects.Add(cst4);
            cst4.SubjectTeacher = subteach3;
            subteach3.TaughtSubjectClasses.Add(cst4);
            csts.Add(cst4);

            ClassSubjectTeacher cst5 = new ClassSubjectTeacher();
            cst5.Class = class5;
            class5.AttendedTeacherSubjects.Add(cst5);
            cst5.SubjectTeacher = subteach11;
            subteach11.TaughtSubjectClasses.Add(cst5);
            csts.Add(cst5);
            

            ClassSubjectTeacher cst6 = new ClassSubjectTeacher();
            cst6.Class = class5;
            class5.AttendedTeacherSubjects.Add(cst6);
            cst6.SubjectTeacher = subteach9;
            subteach9.TaughtSubjectClasses.Add(cst6);
            csts.Add(cst6);

            ClassSubjectTeacher cst7 = new ClassSubjectTeacher();
            cst7.Class = class1;
            class1.AttendedTeacherSubjects.Add(cst7);
            cst7.SubjectTeacher = subteach8;
            subteach8.TaughtSubjectClasses.Add(cst7);
            csts.Add(cst7);

            //ClassSubjectTeacher cst8 = new ClassSubjectTeacher();
            //cst8.Class = class3;
            //class3.AttendedTeacherSubjects.Add(cst8);
            //cst8.SubjectTeacher = subteach4;
            //subteach4.TaughtSubjectClasses.Add(cst8);
            //csts.Add(cst8);

            ClassSubjectTeacher cst9 = new ClassSubjectTeacher();
            cst9.Class = class1;
            class1.AttendedTeacherSubjects.Add(cst9);
            cst9.SubjectTeacher = subteach7;
            subteach7.TaughtSubjectClasses.Add(cst9);
            csts.Add(cst9);
            context.ClassSubjectTeachers.AddRange(csts);

            ClassSubjectTeacher cst10 = new ClassSubjectTeacher();
            cst10.Class = class6;
            class6.AttendedTeacherSubjects.Add(cst10);
            cst10.SubjectTeacher = subteach4;
            subteach4.TaughtSubjectClasses.Add(cst10);
            csts.Add(cst10);

            IList<Grade> grades = new List<Grade>();

            Grade g1 = new Grade();
            g1.GradeValue = 3;
            //g1.Student = st1;
            //g1.SubjectTeacher = subteach1;
            g1.GradeType = GradeType.REGULAR;
            g1.Semester = Semester.FIRST;
            g1.GradeDate = new DateTime(2018, 9, 15);
            g1.Year = st1.Class.Year;
            st1.Grades.Add(g1);
            cst1.Grades.Add(g1);
            grades.Add(g1);

            Grade g2 = new Grade();
            g2.GradeValue = 3;
            //g2.Student = st2;
            //g2.SubjectTeacher = subteach2;
            g2.GradeDate = new DateTime(2018, 10, 23);
            g2.GradeType = GradeType.REGULAR;
            g2.Semester = Semester.FIRST;
            g2.Year = st2.Class.Year;
            st2.Grades.Add(g2);
            cst3.Grades.Add(g2);
            grades.Add(g2);

            Grade g3 = new Grade();
            g3.GradeValue = 3;
            //g3.Student = st2;
            //g3.SubjectTeacher = subteach2;
            g3.GradeDate = new DateTime(2018, 11, 2);
            g3.GradeType = GradeType.REGULAR;
            g3.Semester = Semester.FIRST;
            g3.Year = st2.Class.Year;
            st2.Grades.Add(g3);
            cst3.Grades.Add(g3);
            grades.Add(g3);

            Grade g4 = new Grade();
            g4.GradeValue = 4;
            //g4.Student = st2;
            //g4.SubjectTeacher = subteach3;
            g4.GradeDate = new DateTime(2018, 9, 14);
            g4.GradeType = GradeType.REGULAR;
            g4.Semester = Semester.FIRST;
            g4.Year = st2.Class.Year;
            st2.Grades.Add(g4);
            cst3.Grades.Add(g4);
            grades.Add(g4);

            Grade g5 = new Grade();
            g5.GradeValue = 3;
            //g5.Student = st3;
            //g5.SubjectTeacher = subteach2;
            g5.GradeType = GradeType.REGULAR;
            g5.Semester = Semester.FIRST;
            g5.GradeDate = new DateTime(2018, 10, 14);
            g5.Year = st3.Class.Year;
            st3.Grades.Add(g5);
            cst6.Grades.Add(g5);
            grades.Add(g5);

            Grade g6 = new Grade();
            g6.GradeValue = 5;
            //g6.Student = st4;
            //g6.SubjectTeacher = subteach5;
            g6.GradeType = GradeType.REGULAR;
            g6.Semester = Semester.FIRST;
            g6.GradeDate = new DateTime(2018, 10,23);
            g6.Year = st4.Class.Year;
            st4.Grades.Add(g6);
            cst1.Grades.Add(g6);
            grades.Add(g6);

            Grade g7 = new Grade();
            g7.GradeValue = 2;
            //g6.Student = st4;
            //g6.SubjectTeacher = subteach5;
            g7.GradeDate = new DateTime(2018, 12, 2);
            g7.GradeType = GradeType.REGULAR;
            g7.Semester = Semester.FIRST;
            g7.Year = st9.Class.Year;
            st9.Grades.Add(g7);
            cst4.Grades.Add(g7);
            grades.Add(g7);
            context.Grades.AddRange(grades);

            Grade g8 = new Grade();
            g8.GradeValue = 3;
            //g6.Student = st4;
            //g6.SubjectTeacher = subteach5;
            g8.GradeDate = new DateTime(2019, 2, 2);
            g8.GradeType = GradeType.REGULAR;
            g8.Semester = Semester.SECOND;
            g8.Year = st9.Class.Year;
            st9.Grades.Add(g8);
            cst4.Grades.Add(g8);
            grades.Add(g8);

            Grade g9 = new Grade();
            g9.GradeValue = 3;
            //g6.Student = st4;
            //g6.SubjectTeacher = subteach5;
            g9.GradeDate = new DateTime(2018, 12, 8);
            g9.GradeType = GradeType.REGULAR;
            g9.Semester = Semester.FIRST;
            g9.Year = st8.Class.Year;
            st8.Grades.Add(g9);
            cst7.Grades.Add(g9);
            grades.Add(g9);

            Grade g10 = new Grade();
            g10.GradeValue = 5;
            //g6.Student = st4;
            //g6.SubjectTeacher = subteach5;
            g10.GradeDate = new DateTime(2018, 2, 3);
            g10.GradeType = GradeType.REGULAR;
            g10.Semester = Semester.SECOND;
            g10.Year = st8.Class.Year;
            st8.Grades.Add(g10);
            cst9.Grades.Add(g10);
            grades.Add(g10);

            Grade g11 = new Grade();
            g11.GradeValue = 3;
            //g6.Student = st4;
            //g6.SubjectTeacher = subteach5;
            g11.GradeDate = new DateTime(2018, 2, 4);
            g11.GradeType = GradeType.REGULAR;
            g11.Semester = Semester.SECOND;
            g11.Year = st9.Class.Year;
            st9.Grades.Add(g11);
            cst10.Grades.Add(g11);
            grades.Add(g11);

            //Grade g12 = new Grade();
            //g12.GradeValue = 5;
            ////g6.Student = st4;
            ////g6.SubjectTeacher = subteach5;
            //g12.GradeDate = new DateTime(2019, 12, 10);
            //g12.GradeType = GradeType.HALFYEAR;
            //g12.Semester = Semester.FIRST;
            //g12.Year = st8.Class.Year;
            //st8.Grades.Add(g12);
            //cst9.Grades.Add(g12);
            //grades.Add(g12);

            Grade g13 = new Grade();
            g13.GradeValue = 5;
            //g6.Student = st4;
            //g6.SubjectTeacher = subteach5;
            g13.GradeDate = new DateTime(2019, 12, 11);
            g13.GradeType = GradeType.FINAL;
            g13.Semester = Semester.FIRST;
            g13.Year = st8.Class.Year;
            st8.Grades.Add(g13);
            cst7.Grades.Add(g13);
            grades.Add(g13);


            Grade g14 = new Grade();
            g14.GradeValue = 5;
            //g3.Student = st2;
            //g3.SubjectTeacher = subteach2;
            g14.GradeDate = new DateTime(2018, 10, 3);
            g14.GradeType = GradeType.REGULAR;
            g14.Semester = Semester.FIRST;
            g14.Year = st2.Class.Year;
            st2.Grades.Add(g14);
            cst3.Grades.Add(g14);
            grades.Add(g14);

            Grade g15 = new Grade();
            g15.GradeValue = 5;
            //g3.Student = st2;
            //g3.SubjectTeacher = subteach2;
            g15.GradeDate = new DateTime(2018, 10, 4);
            g15.GradeType = GradeType.REGULAR;
            g15.Semester = Semester.FIRST;
            g15.Year = st2.Class.Year;
            st2.Grades.Add(g15);
            cst7.Grades.Add(g15);
            grades.Add(g15);

            Grade g16 = new Grade();
            g16.GradeValue = 5;
            g16.GradeDate = new DateTime(2018, 10, 5);
            g16.GradeType = GradeType.REGULAR;
            g16.Semester = Semester.FIRST;
            g16.Year = st2.Class.Year;
            st2.Grades.Add(g16);
            cst3.Grades.Add(g16);
            grades.Add(g16);

            //i
            Grade g17 = new Grade();
            g17.GradeValue = 3;
            g17.GradeDate = new DateTime(2018, 10, 6);
            g17.GradeType = GradeType.REGULAR;
            g17.Semester = Semester.FIRST;
            g17.Year = st4.Class.Year;
            st4.Grades.Add(g17);
            cst1.Grades.Add(g17);
            grades.Add(g17);

            Grade g20 = new Grade();
            g20.GradeValue = 3;
            g20.GradeDate = new DateTime(2018, 10, 7);
            g20.GradeType = GradeType.REGULAR;
            g20.Semester = Semester.FIRST;
            g20.Year = st8.Class.Year;
            st8.Grades.Add(g20);
            cst1.Grades.Add(g20);
            grades.Add(g20);

            Grade g18 = new Grade();
            g18.GradeValue = 3;
            g18.GradeDate = new DateTime(2018, 2, 4);
            g18.GradeType = GradeType.REGULAR;
            g18.Semester = Semester.SECOND;
            g18.Year = st8.Class.Year;
            st8.Grades.Add(g18);
            cst1.Grades.Add(g18);
            grades.Add(g18);

            Grade g19 = new Grade();
            g19.GradeValue = 3;
            g19.GradeDate = new DateTime(2018, 1, 29);
            g19.GradeType = GradeType.REGULAR;
            g19.Semester = Semester.SECOND;
            g19.Year = st2.Class.Year;
            st2.Grades.Add(g19);
            cst3.Grades.Add(g19);
            grades.Add(g19);


            Grade g21 = new Grade();
            g21.GradeValue = 3;
            g21.GradeDate = new DateTime(2018, 1, 11);
            g21.GradeType = GradeType.REGULAR;
            g21.Semester = Semester.SECOND;
            g21.Year = st1.Class.Year;
            st1.Grades.Add(g21);
            cst9.Grades.Add(g21);
            grades.Add(g21);


            Grade g22 = new Grade();
            g22.GradeValue = 2;
            g22.GradeDate = new DateTime(2018, 7, 11);
            g22.GradeType = GradeType.REGULAR;
            g22.Semester = Semester.FIRST;
            g22.Year = st1.Class.Year;
            st4.Grades.Add(g22);
            cst7.Grades.Add(g22);
            grades.Add(g22);

            Grade g23 = new Grade();
            g23.GradeValue = 4;
            g23.GradeDate = new DateTime(2019, 2, 3);
            g23.GradeType = GradeType.REGULAR;
            g23.Semester = Semester.SECOND;
            g23.Year = st1.Class.Year;
            st4.Grades.Add(g23);
            cst7.Grades.Add(g23);
            grades.Add(g23);

            Grade g24 = new Grade();
            g24.GradeValue = 4;
            g24.GradeDate = new DateTime(2019, 2, 3);
            g24.GradeType = GradeType.REGULAR;
            g24.Semester = Semester.SECOND;
            g24.Year = st12.Class.Year;
            st12.Grades.Add(g24);
            cst3.Grades.Add(g24);
            grades.Add(g24);

            Grade g25 = new Grade();
            g25.GradeValue = 2;
            g25.GradeDate = new DateTime(2019, 2,4);
            g25.GradeType = GradeType.REGULAR;
            g25.Semester = Semester.SECOND;
            g25.Year = st12.Class.Year;
            st12.Grades.Add(g25);
            cst3.Grades.Add(g25);
            grades.Add(g25);

            context.Grades.AddRange(grades);

            context.SaveChanges();
        }
    }
}
