using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;

using Projekat;
using Projekat.Infrastructure;
using Projekat.Models;
using Projekat.Providers;
using Projekat.Repositories;
using Projekat.Repository;
using Projekat.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;
using Projekat.Models.DTOs;
using AutoMapper;
using Projekat.ModelsIzmena.DTOs;


[assembly: OwinStartup(typeof(Projekat.Startup))]
    namespace Projekat
{
        public class Startup
        {
        public void Configuration(IAppBuilder app)
        {
            var container = SetupUnity();
            ConfigureOAuth(app, container);

            HttpConfiguration config = new HttpConfiguration();
            config.DependencyResolver = new UnityDependencyResolver(container);
            //config.EnableCors();

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            WebApiConfig.Register(config);
            app.UseWebApi(config);

            //podesavanje za datum
            config.Formatters.JsonFormatter.SerializerSettings.DateFormatString = "dd.MM.yyyy";
           
            Mapper.Initialize(cfg => {
                
                cfg.CreateMap<Class, ClassDTO>().ReverseMap();
              
                cfg.CreateMap<Grade, GradeCreateDTO>().ReverseMap();
               
                cfg.CreateMap<ApplicationUser, UserDTO>()
                .Include<Student, StudentDTO>()
                .Include<Teacher, TeacherDTO>()
                .Include<Admin, AdminDTO>();

                cfg.CreateMap<UserDTO, ApplicationUser>()
                .Include<StudentDTO, Student>()
                .Include<TeacherDTO, Teacher>()
                .Include<ParentDTO, Parent>()
                .Include<AdminDTO, Admin>();

                cfg.CreateMap<UserPrivateDTO, ApplicationUser>()
                .Include<StudentPrivateDTO, Student>();

                cfg.CreateMap<ApplicationUser, UserPrivateDTO>()
               .Include<Student, StudentPrivateDTO>();

                cfg.CreateMap<Student, StudentDTO>();
                cfg.CreateMap<StudentDTO, Student>();

                cfg.CreateMap<Student, StudentPrivateDTO>();
                cfg.CreateMap<StudentPrivateDTO, Student>();

                cfg.CreateMap<Student, StudentTDTO>().ReverseMap();
                cfg.CreateMap<Student, StudentPDTO>().ReverseMap();

                cfg.CreateMap<Admin, AdminDTO>().ReverseMap();

                cfg.CreateMap<Subject, SubjectDTO>().ReverseMap();

                cfg.CreateMap<Subject, SubjectUpdateDTO>().ReverseMap();

                cfg.CreateMap<Teacher, TeacherDTO>().ReverseMap();
                cfg.CreateMap<Teacher, TeacherPDTO>().ReverseMap();
                cfg.CreateMap<Teacher, TeacherTDTO>().ReverseMap();

                cfg.CreateMap<Parent, ParentDTO>().ReverseMap();
                cfg.CreateMap<Parent, ParentPDTO>().ReverseMap();
                cfg.CreateMap<Parent, ParentTDTO>().ReverseMap();

                cfg.CreateMap<SubjectTeacher, SubjectTeacherDTO>().ReverseMap();
                cfg.CreateMap<ClassSubjectTeacher, ClassSubjectTeacherDTO>().ReverseMap();

                cfg.CreateMap<Grade, GradeDTO>().ReverseMap();
               

                cfg.CreateMap<ApplicationUser, UserRegistrationDTO>()
                .Include<Student, StudentRegistrationDTO>()
                .Include<Teacher, TeacherRegistrationDTO>()
                
                .Include<Admin, AdminRegistrationDTO>();

                cfg.CreateMap<UserRegistrationDTO, ApplicationUser>()
                .Include<StudentRegistrationDTO, Student>()
                .Include<TeacherRegistrationDTO, Teacher>()
                
                .Include<AdminRegistrationDTO, Admin>();

                cfg.CreateMap<Admin, AdminRegistrationDTO>().ReverseMap();
                //cfg.CreateMap<AdminRegistrationDTO, Admin>();

                cfg.CreateMap<Student, StudentRegistrationDTO>().ReverseMap();
                //cfg.CreateMap<StudentRegistrationDTO, Student>();

                cfg.CreateMap<Teacher, TeacherRegistrationDTO>().ReverseMap();
                //cfg.CreateMap<TeacherRegistrationDTO, Teacher>();

                cfg.CreateMap<Parent, ParentRegistrationDTO>().ReverseMap();
                //cfg.CreateMap<ParentRegistrationDTO, Parent>();

            });
        }

        public void ConfigureOAuth(IAppBuilder app, UnityContainer container)
        {
            //app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/project/login"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(3),
                Provider = new SimpleAuthorizationServerProvider(container)
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        }
        private UnityContainer SetupUnity()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.RegisterType<IGenericRepository<ApplicationUser>, GenericRepository<ApplicationUser>>();
            container.RegisterType<IAuthRepository, AuthRepository>();          
            container.RegisterType<IGenericRepository<Parent>, GenericRepository<Parent>>();          
            container.RegisterType<IGenericRepository<Admin>, GenericRepository<Admin>>();
            
            container.RegisterType<IClassesRepository, ClassesRepository>();
            container.RegisterType<ISubjectsRepository, SubjectsRepository>();
            container.RegisterType<ISubjectTeachersRepository, SubjectTeachersRepository>();
            container.RegisterType<IClassSubjectTeacherRepository, ClassSubjectTeachersRepository>();
            container.RegisterType<IGradesRepository, GradesRepository>();
            container.RegisterType<IStudentsRepository, StudentsRepository>();
            container.RegisterType<ITeachersRepository, TeachersRepository>();

            container.RegisterType<DbContext, AuthContext>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserRegistrationService, UserRegistrationService>();
            container.RegisterType<IClassService, ClassService>();
            container.RegisterType<IStudentService, StudentService>();
            container.RegisterType<IParentService, ParentService>();
            container.RegisterType<ISubjectService, SubjectService>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<ITeacherService, TeacherService>();
            container.RegisterType<ISubjectTeacherService, SubjectTeacherService>();
            container.RegisterType<IEmailService, EmailService>();
            container.RegisterType<IClassSubjectTeacherService, ClassSubjectTeacherService>();
            container.RegisterType<IGradeService, GradeService>();
            container.RegisterType<IAdminService, AdminService>();

            
            return container;
        }
    }
}
