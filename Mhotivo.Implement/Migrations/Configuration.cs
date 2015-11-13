using System;
using System.Collections.Generic;
using Mhotivo.Data.Entities;
using Mhotivo.Implement.Context;
using Mhotivo.Implement.Repositories;
using Mhotivo.Interface.Interfaces;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Mhotivo.Implement.Migrations
{
    public class Configuration : DbMigrationsConfiguration<MhotivoContext>
    {
        private IEducationLevelRepository _areaRepository;
        private IGradeRepository _gradeRepository;
        private ICourseRepository _courseRepository;
        private IPensumRepository _pensumRepository;
        private IAcademicYearRepository _academicYearRepository;
        private IRoleRepository _roleRepository;
        private ITeacherRepository _teacherRepository;
        private INotificationTypeRepository _notificationTypeRepository;
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(MhotivoContext context)
        {
            if (context.Users.Any())
                return;
            _areaRepository = new EducationLevelRepository(context);
            _gradeRepository = new GradeRepository(context);
            _courseRepository = new CourseRepository(context);
            _pensumRepository = new PensumRepository(context);
            _academicYearRepository = new AcademicYearRepository(context);
            _roleRepository = new RoleRepository(context);
            _teacherRepository = new TeacherRepository(context);
            _notificationTypeRepository = new NotificationTypeRepository(context);
            _roleRepository.Create(new Role { Name = "Administrador", Privileges = new HashSet<Privilege>(), RoleId = 0 });
            _roleRepository.Create(new Role { Name = "Padre", Privileges = new HashSet<Privilege>(), RoleId = 1 });
            _roleRepository.Create(new Role { Name = "Maestro", Privileges = new HashSet<Privilege>(), RoleId = 2 });
            _roleRepository.Create(new Role { Name = "Director", Privileges = new HashSet<Privilege>(), RoleId = 3 });
            var admin = new User
            {
                DisplayName = "Administrador",
                Email = "admin@mhotivo.org",
                Password = "password",
                IsActive = true,
                Role = _roleRepository.FirstOrDefault(x => x.Name == "Administrador")
            };
            admin.HashPassword();
            context.Users.AddOrUpdate(admin);
            context.SaveChanges();
            context.NotificationTypes.AddOrUpdate(new NotificationType { Id = 1, Description = "General" });
            context.NotificationTypes.AddOrUpdate(new NotificationType { Id = 2, Description = "Nivel De Educacion" });
            context.NotificationTypes.AddOrUpdate(new NotificationType { Id = 3, Description = "Grado" });
            context.NotificationTypes.AddOrUpdate(new NotificationType { Id = 4, Description = "Personal" });
            context.SaveChanges();
            DebuggingSeeder(context);
            context.SaveChanges();
        }

        private void DebuggingSeeder(MhotivoContext context)
        {
            _areaRepository.Create(new EducationLevel { Name = "Prescolar" });
            _areaRepository.Create(new EducationLevel { Name = "Primaria" });
            _areaRepository.Create(new EducationLevel { Name = "Secundaria" });
            _gradeRepository.Create(new Grade { EducationLevel = _areaRepository.GetById(1).Name, Name = "Kinder" });
            _gradeRepository.Create(new Grade { EducationLevel = _areaRepository.GetById(1).Name, Name = "Preparatoria" });
            _gradeRepository.Create(new Grade { EducationLevel = _areaRepository.GetById(2).Name, Name = "Primero" });
            _gradeRepository.Create(new Grade { EducationLevel = _areaRepository.GetById(2).Name, Name = "Segundo" });
            _gradeRepository.Create(new Grade { EducationLevel = _areaRepository.GetById(2).Name, Name = "Tercero" });
            _gradeRepository.Create(new Grade { EducationLevel = _areaRepository.GetById(2).Name, Name = "Cuarto" });
            _gradeRepository.Create(new Grade { EducationLevel = _areaRepository.GetById(2).Name, Name = "Quinto" });
            _gradeRepository.Create(new Grade { EducationLevel = _areaRepository.GetById(2).Name, Name = "Sexto" });
            _gradeRepository.Create(new Grade { EducationLevel = _areaRepository.GetById(3).Name, Name = "Septimo" });
            _gradeRepository.Create(new Grade { EducationLevel = _areaRepository.GetById(3).Name, Name = "Octavo" });
            _gradeRepository.Create(new Grade { EducationLevel = _areaRepository.GetById(3).Name, Name = "Noveno" });
            _gradeRepository.Create(new Grade { EducationLevel = _areaRepository.GetById(3).Name, Name = "Decimo" });
            _gradeRepository.Create(new Grade { EducationLevel = _areaRepository.GetById(3).Name, Name = "Onceavo" });
            _courseRepository.Create(new Course { Area = _areaRepository.GetById(1), Name = "English" });
            _courseRepository.Create(new Course { Area = _areaRepository.GetById(1), Name = "Math" });
            _courseRepository.Create(new Course { Area = _areaRepository.GetById(2), Name = "English" });
            _courseRepository.Create(new Course { Area = _areaRepository.GetById(2), Name = "Math" });
            _courseRepository.Create(new Course { Area = _areaRepository.GetById(2), Name = "Science" });
            _courseRepository.Create(new Course { Area = _areaRepository.GetById(2), Name = "Espaniol" });
            _courseRepository.Create(new Course { Area = _areaRepository.GetById(2), Name = "Estudios Sociales" });
            _courseRepository.Create(new Course { Area = _areaRepository.GetById(3), Name = "Algebra" });
            _courseRepository.Create(new Course { Area = _areaRepository.GetById(3), Name = "Geometry" });
            _courseRepository.Create(new Course { Area = _areaRepository.GetById(3), Name = "Physics" });
            _courseRepository.Create(new Course { Area = _areaRepository.GetById(3), Name = "Biology" });
            _courseRepository.Create(new Course { Area = _areaRepository.GetById(3), Name = "Physical Education" });
            _pensumRepository.Create(new Pensum { Course = _courseRepository.GetById(1), Grade = _gradeRepository.GetById(1) });
            _pensumRepository.Create(new Pensum { Course = _courseRepository.GetById(1), Grade = _gradeRepository.GetById(2) });
            _pensumRepository.Create(new Pensum { Course = _courseRepository.GetById(2), Grade = _gradeRepository.GetById(2) });
            _pensumRepository.Create(new Pensum { Course = _courseRepository.GetById(2), Grade = _gradeRepository.GetById(1) });
            _pensumRepository.Create(new Pensum { Course = _courseRepository.GetById(3), Grade = _gradeRepository.GetById(3) });
            _pensumRepository.Create(new Pensum { Course = _courseRepository.GetById(3), Grade = _gradeRepository.GetById(4) });
            _pensumRepository.Create(new Pensum { Course = _courseRepository.GetById(4), Grade = _gradeRepository.GetById(5) });
            _pensumRepository.Create(new Pensum { Course = _courseRepository.GetById(5), Grade = _gradeRepository.GetById(6) });
            _pensumRepository.Create(new Pensum { Course = _courseRepository.GetById(6), Grade = _gradeRepository.GetById(7) });
            _pensumRepository.Create(new Pensum { Course = _courseRepository.GetById(7), Grade = _gradeRepository.GetById(8) });
            _pensumRepository.Create(new Pensum { Course = _courseRepository.GetById(8), Grade = _gradeRepository.GetById(9) });
            _pensumRepository.Create(new Pensum { Course = _courseRepository.GetById(12), Grade = _gradeRepository.GetById(10) });
            _pensumRepository.Create(new Pensum { Course = _courseRepository.GetById(9), Grade = _gradeRepository.GetById(11) });
            _pensumRepository.Create(new Pensum { Course = _courseRepository.GetById(10), Grade = _gradeRepository.GetById(12) });
            _pensumRepository.Create(new Pensum { Course = _courseRepository.GetById(11), Grade = _gradeRepository.GetById(13) });
            for (int i = 1; i <= 13; i++)
            {
                _academicYearRepository.Create(new AcademicYear { Approved = true, Grade = _gradeRepository.GetById(i), IsActive = true, Section = "A", Year = 2015 });
                _academicYearRepository.Create(new AcademicYear { Approved = true, Grade = _gradeRepository.GetById(i), IsActive = true, Section = "B", Year = 2015 });
                _academicYearRepository.Create(new AcademicYear { Approved = true, Grade = _gradeRepository.GetById(i), IsActive = true, Section = "C", Year = 2015 });
            }
            var genericTeacher = new User
            {
                DisplayName = "Maestro Generico",
                Email = "teacher@mhotivo.org",
                Password = "password",
                IsActive = true,
                Role = _roleRepository.FirstOrDefault(x => x.Name == "Maestro")
            };
            genericTeacher.HashPassword();
            var genericParent = new User
            {
                DisplayName = "Padre Generico",
                Email = "parent@mhotivo.org",
                Password = "password",
                IsActive = true,
                Role = _roleRepository.FirstOrDefault(x => x.Name == "Padre")
            };
            genericParent.HashPassword();
            var genericMom = new User
            {
                DisplayName = "Madre Generica",
                Email = "mom@mhotivo.org",
                Password = "password",
                IsActive = true,
                Role = _roleRepository.FirstOrDefault(x => x.Name == "Padre")
            };
            genericMom.HashPassword();
            context.Users.AddOrUpdate(genericTeacher);
            context.Users.AddOrUpdate(genericParent);
            context.SaveChanges();

            var maestroDefault = context.Teachers.FirstOrDefault(x => x.FullName == "Maestro Generico");
            if (maestroDefault == null)
            {
                context.Teachers.AddOrUpdate(new Teacher { IdNumber = "0000000000000", FirstName = "Maestro", LastName = "Generico", FullName = "Maestro Generico", Disable = false, MyGender = Gender.Masculino, MyUser = genericTeacher });
            }
            var padreDefault = context.Parents.FirstOrDefault(x => x.FullName == "Padre Generico");
            if (padreDefault == null)
            {
                context.Parents.AddOrUpdate(new Parent { IdNumber = "1234567890", FirstName = "Padre", LastName = "Generico", FullName = "Padre Generico", Disable = false, MyGender = Gender.Masculino, MyUser = genericParent });
            }
            var madreDefault = context.Parents.FirstOrDefault(x => x.FullName == "Madre Generica");
            if (madreDefault == null)
            {
                context.Parents.AddOrUpdate(new Parent { IdNumber = "1234567133", FirstName = "Madre", LastName = "Generica", FullName = "Madre Generica", Disable = false, MyGender = Gender.Femenino, MyUser = genericMom });
            }
            context.SaveChanges();
            var student = new Student()
            {
                Id = 4,
                IdNumber = "8153-7946-98768",
                FirstName = "Hans",
                LastName = "Landa",
                AccountNumber = "21241103",
                Address = "Address No One Cares about",
                Biography = "none",
                BirthDate = "11/4/2015",
                BloodType = "O+",
                City = "Frankfurt",
                FullName = "Hans Landa",
                Nationality = "Aleman",
                State = "Frankfurt",
                Country = "Germany",
                MyGender = Gender.Masculino,
                StartDate = "11-11-2015",
                Tutor1 = context.Parents.FirstOrDefault(x => x.FullName == "Padre Generico"),
                Tutor2 = context.Parents.FirstOrDefault(x => x.FullName == "Madre Generica")
            };
            context.Students.Add(student);
            context.SaveChanges();

            var enroll = new Enroll()
            {
                Id = 1,
                AcademicYear = _academicYearRepository.GetById(25),
                Student = student
            };
            context.Enrolls.Add(enroll);
            context.SaveChanges();

            var detail = new AcademicYearDetail()
            {
                Id = 1,
                TeacherStartDate = DateTime.Today,
                TeacherEndDate = DateTime.Today,
                Schedule = DateTime.Today,
                Room = "445",
                AcademicYear = _academicYearRepository.GetById(25),
                Course = _courseRepository.GetById(8),
                Teacher = _teacherRepository.GetById(1)
            };
            context.AcademicYearDetails.Add(detail);
            context.SaveChanges();

            var hw = new Homework()
            {
                Id = 1,
                DeliverDate = DateTime.Today,
                Description = "<p>Testing Homeworks!</p>",
                Points = 15,
                Title = "HW Test",
                AcademicYearDetail = detail
            };
            context.Homeworks.Add(hw);
            context.SaveChanges();

            var testNotification = new Notification()
            {
                Id = 1,
                Approved = true,
                Created = DateTime.Today,
                GradeIdifNotificationTypePersonal = 0,
                IdGradeAreaUserGeneralSelected = 0,
                Message = "Hi, Im Glen!",
                NotificationComments = null,
                NotificationCreator = genericTeacher,
                NotificationName = "Testing Notifications",
                NotificationType = _notificationTypeRepository.GetById(3),
                Section = "A",
                SendingEmail = false,
                TargetStudent = null,
                UserCreatorId = genericTeacher.Id,
                UserCreatorName = genericTeacher.DisplayName,
                Users = new List<User>()
            };
            context.Notifications.Add(testNotification);

            var testgeneralNotification = new Notification()
            {
                Id = 2,
                Approved = true,
                Created = DateTime.Today,
                GradeIdifNotificationTypePersonal = 0,
                IdGradeAreaUserGeneralSelected = 0,
                Message = "<p>Testing General Notifications</p>!",
                NotificationComments = null,
                NotificationCreator = genericTeacher,
                NotificationName = "Testing General Notifications",
                NotificationType = _notificationTypeRepository.GetById(1),
                Section = "Todos",
                SendingEmail = false,
                TargetStudent = null,
                UserCreatorId = genericTeacher.Id,
                UserCreatorName = genericTeacher.DisplayName,
                Users = new List<User>()
            };
            context.Notifications.Add(testgeneralNotification);


            var testLevelNotification = new Notification()
            {
                Id = 3,
                Approved = true,
                Created = DateTime.Today,
                GradeIdifNotificationTypePersonal = 0,
                IdGradeAreaUserGeneralSelected = 3,
                Message = "<p>Testing Level Notification<br></p>",
                NotificationComments = null,
                NotificationCreator = genericTeacher,
                NotificationName = "Testing Level Notification",
                NotificationType = _notificationTypeRepository.GetById(2),
                Section = "Todos",
                SendingEmail = false,
                TargetStudent = null,
                UserCreatorId = genericTeacher.Id,
                UserCreatorName = genericTeacher.DisplayName,
                Users = new List<User>()
            };
            context.Notifications.Add(testLevelNotification);

            var testPersonalNotification = new Notification()
            {
                Id = 4,
                Approved = true,
                Created = DateTime.Today,
                GradeIdifNotificationTypePersonal = 0,
                IdGradeAreaUserGeneralSelected = 4,
                Message = "Testing Personal Notification",
                NotificationComments = null,
                NotificationCreator = genericTeacher,
                NotificationName = "Testing Personal Notification",
                NotificationType = _notificationTypeRepository.GetById(4),
                Section = "A",
                SendingEmail = false,
                TargetStudent = student,
                UserCreatorId = genericTeacher.Id,
                UserCreatorName = genericTeacher.DisplayName,
                Users = new List<User>()
            };

            context.Notifications.Add(testPersonalNotification);
            context.SaveChanges();
        }
    }
}