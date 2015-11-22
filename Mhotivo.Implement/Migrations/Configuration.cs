using System;
using Mhotivo.Data.Entities;
using Mhotivo.Implement.Context;
using Mhotivo.Implement.Repositories;
using Mhotivo.Interface.Interfaces;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Mhotivo.Implement.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<MhotivoContext>
    {
        private IEducationLevelRepository _areaRepository;
        private IGradeRepository _gradeRepository;
        private ICourseRepository _courseRepository;
        private IPensumRepository _pensumRepository;
        private IAcademicYearRepository _academicYearRepository;
        private IRoleRepository _roleRepository;
        private IUserRepository _userRepository;
        private ITeacherRepository _teacherRepository;
        private ITutorRepository _tutorRepository;
        private IAcademicGradeRepository _academicGradeRepository;
        private IAcademicCourseRepository _academicCourseRepository;
        private IPeopleWithUserRepository _peopleWithUserRepository;

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
            _userRepository = new UserRepository(context);
            _teacherRepository = new TeacherRepository(context);
            _tutorRepository = new TutorRepository(context);
            _academicGradeRepository = new AcademicGradeRepository(context);
            _academicCourseRepository = new AcademicCourseRepository(context);
            _peopleWithUserRepository = new PeopleWithUserRepository(context);

            _roleRepository.Create(new Role { Name = "Administrador", Id = 0 });
            _roleRepository.Create(new Role { Name = "Tutor", Id = 1 });
            _roleRepository.Create(new Role { Name = "Maestro", Id = 2 });
            _roleRepository.Create(new Role { Name = "Director", Id = 3 });
            _roleRepository.Create(new Role { Name = "Maestro de Seccion", Id = 4 });
            var adminPeople = new PeopleWithUser
            {
                Address = "",
                BirthDate = DateTime.UtcNow,
                City = "",
                FirstName = "Administrador",
                IsActive = true,
                IdNumber = "0000-0000-00000",
                LastName = "",
                MyGender = Gender.Masculino,
                Photo = null,
                State = ""
            };
            adminPeople.FullName = adminPeople.FirstName + "" + adminPeople.LastName;
            adminPeople = _peopleWithUserRepository.Create(adminPeople);
            var admin = new User
            {
                Email = "admin@mhotivo.org",
                Password = "password",
                IsActive = true,
                UserOwner = adminPeople,
                Role = _roleRepository.Filter(x => x.Name == "Administrador").FirstOrDefault()
            };
            admin = _userRepository.Create(admin);
            adminPeople.User = admin;
            _peopleWithUserRepository.Update(adminPeople);
            DebuggingSeeder();
        }

        private void DebuggingSeeder()
        {
            _areaRepository.Create(new EducationLevel { Name = "Pre-Escolar" });
            _areaRepository.Create(new EducationLevel { Name = "Primaria" });
            _areaRepository.Create(new EducationLevel { Name = "Secundaria" });
            _gradeRepository.Create(new Grade { EducationLevel = _areaRepository.GetById(1), Name = "Kinder" });
            _gradeRepository.Create(new Grade { EducationLevel = _areaRepository.GetById(1), Name = "Preparatoria" });
            _gradeRepository.Create(new Grade { EducationLevel = _areaRepository.GetById(2), Name = "Primero" });
            _gradeRepository.Create(new Grade { EducationLevel = _areaRepository.GetById(2), Name = "Segundo" });
            _gradeRepository.Create(new Grade { EducationLevel = _areaRepository.GetById(2), Name = "Tercero" });
            _gradeRepository.Create(new Grade { EducationLevel = _areaRepository.GetById(2), Name = "Cuarto" });
            _gradeRepository.Create(new Grade { EducationLevel = _areaRepository.GetById(2), Name = "Quinto" });
            _gradeRepository.Create(new Grade { EducationLevel = _areaRepository.GetById(2), Name = "Sexto" });
            _gradeRepository.Create(new Grade { EducationLevel = _areaRepository.GetById(3), Name = "Septimo" });
            _gradeRepository.Create(new Grade { EducationLevel = _areaRepository.GetById(3), Name = "Octavo" });
            _gradeRepository.Create(new Grade { EducationLevel = _areaRepository.GetById(3), Name = "Noveno" });
            _gradeRepository.Create(new Grade { EducationLevel = _areaRepository.GetById(3), Name = "Decimo" });
            _gradeRepository.Create(new Grade { EducationLevel = _areaRepository.GetById(3), Name = "Onceavo" });

            foreach (var grade in _gradeRepository.GetAllGrade())
            {
                var p = new Pensum{ Grade = grade, Name = "Pensum para " + grade.Name};
                p = _pensumRepository.Create(p);
                grade.Pensums.Add(p);
                _gradeRepository.Update(grade);
                var c1 = new Course {Name = "Ingles para " + grade.Name, Pensum = p};
                var c2 = new Course { Name = "Matematicas para " + grade.Name, Pensum = p };
                var c3 = new Course { Name = "Ciencias para " + grade.Name, Pensum = p };
                _courseRepository.Create(c1);
                _courseRepository.Create(c2);
                _courseRepository.Create(c3);
                p.Courses.Add(c1);
                p.Courses.Add(c2);
                p.Courses.Add(c3);
                _pensumRepository.Update(p);
            }

            var generTeacher = new Teacher
            {
                Address = "Jardines del Valle, 4 calle, 1 etapa",
                BirthDate = new DateTime(1993, 3, 8),
                City = "San Pedro Sula",
                IsActive = true,
                FirstName = "Jose",
                LastName = "Avila",
                FullName = "Jose Avila",
                IdNumber = "0501-1993-08405",
                MyGender = Gender.Masculino,
                State = "Cortes",
            };
            generTeacher = _teacherRepository.Create(generTeacher);
            var genericTeacher = new User
            {
                UserOwner = generTeacher,
                Email = "teacher@mhotivo.org",
                Password = "password",
                IsActive = true,
                Role = _roleRepository.Filter(x => x.Name == "Maestro").FirstOrDefault(),
                IsUsingDefaultPassword = false
            };
            genericTeacher = _userRepository.Create(genericTeacher);
            generTeacher.User = genericTeacher;
            generTeacher = _teacherRepository.Update(generTeacher);
            var academicYear = new AcademicYear { IsActive = true, Year = 2015, EnrollsOpen = true};
            academicYear = _academicYearRepository.Create(academicYear);
            for (int i = 1; i <= 13; i++)
            {
                var rnd = new Random();
                char section = 'A';
                do
                {
                    var grade = _gradeRepository.GetById(i);
                    var newGrade = new AcademicGrade
                    {
                        Grade = grade,
                        AcademicYear = academicYear,
                        Section = section++ + "",
                        ActivePensum = grade.Pensums.ElementAt(0)
                    };
                    newGrade = _academicGradeRepository.Create(newGrade);
                    foreach (var course in newGrade.ActivePensum.Courses)
                    {
                        var academicCourse = new AcademicCourse
                        {
                            Course = course,
                            AcademicGrade = newGrade,
                            Schedule = newGrade.CoursesDetails.Any() ? 
                            newGrade.CoursesDetails.Last().Schedule.Duration().Add(new TimeSpan(0, 40, 0)) : 
                            new TimeSpan(7, 0, 0),
                            Teacher = generTeacher //TODO: Create more teachers to randomize this a little bit more?
                        };
                        academicCourse = _academicCourseRepository.Create(academicCourse);
                        newGrade.CoursesDetails.Add(academicCourse);
                        newGrade = _academicGradeRepository.Update(newGrade);
                    }
                    academicYear.Grades.Add(newGrade);
                    _academicYearRepository.Update(academicYear);
                } while (rnd.Next(0, 2) == 0);
            }
            var generTutor = new Tutor
            {
                IdNumber = "0501-1956-03145",
                FirstName = "Tutor",
                LastName = "Generico",
                FullName = "Tutor Generico",
                IsActive = true,
                MyGender = Gender.Femenino,
                BirthDate = new DateTime(1956, 11, 23),
                Parentage = Parentage.Mother,
                City = "San Pedro Sula",
                State = "Cortes",
                Address = "Jardines del Valle, 4 Calle, 1 Etapa, #9D",
            };
            generTutor = _tutorRepository.Create(generTutor);
            var genericTutor = new User
            {
                UserOwner = generTutor,
                Email = "tutor@mhotivo.org",
                Password = "password",
                IsActive = true,
                Role = _roleRepository.Filter(x => x.Name == "Tutor").FirstOrDefault()
            };
            genericTutor = _userRepository.Create(genericTutor);
            generTutor.User = genericTutor;
            _tutorRepository.Update(generTutor);

            var director = new PeopleWithUser
            {
                Address = "",
                BirthDate = DateTime.UtcNow,
                City = "",
                FirstName = "Director Generico",
                IsActive = true,
                IdNumber = "0000-0000-00000",
                LastName = "",
                MyGender = Gender.Masculino,
                Photo = null,
                State = "",
            };
            director.FullName = director.FirstName + "" + director.LastName;
            director = _peopleWithUserRepository.Create(director);
            var dir = new User
            {
                Email = "director@mhotivo.org",
                Password = "password",
                IsActive = true,
                UserOwner = director,
                Role = _roleRepository.Filter(x => x.Name == "Director").FirstOrDefault()
            };
            dir = _userRepository.Create(dir);
            director.User = dir;
            _peopleWithUserRepository.Update(director);
        }
    }
}
