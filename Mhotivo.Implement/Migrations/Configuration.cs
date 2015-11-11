using System;
using System.Collections.Generic;
using System.Globalization;
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
        private IUserRepository _userRepository;
        private ITeacherRepository _teacherRepository;
        private IParentRepository _parentRepository;
        private IAcademicYearGradeRepository _academicYearGradeRepository;
        private IAcademicYearCourseRepository _academicYearCourseRepository;

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
            _parentRepository = new ParentRepository(context);
            _academicYearGradeRepository = new AcademicYearGradeRepository(context);
            _academicYearCourseRepository = new AcademicYearCourseRepository(context);

            _roleRepository.Create(new Role { Name = "Administrador", Id = 0 });
            _roleRepository.Create(new Role { Name = "Padre", Id = 1 });
            _roleRepository.Create(new Role { Name = "Maestro", Id = 2 });
            _roleRepository.Create(new Role { Name = "Director", Id = 3 });
            _roleRepository.Create(new Role { Name = "Maestro de Seccion", Id = 4 });

            var admin = new User
            {
                DisplayName = "Administrador",
                Email = "admin@mhotivo.org",
                Password = "password",
                IsActive = true,
                Role = _roleRepository.Filter(x => x.Name == "Administrador").FirstOrDefault()
            };
            _userRepository.Create(admin);
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
                var p = new Pensum{ Grade = grade };
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

            var genericTeacher = new User
            {
                DisplayName = "Maestro Generico",
                Email = "teacher@mhotivo.org",
                Password = "password",
                IsActive = true,
                Role = _roleRepository.Filter(x => x.Name == "Maestro").FirstOrDefault(),
                IsUsingDefaultPassword = false
            };
            genericTeacher = _userRepository.Create(genericTeacher);
            var generTeacher = new Teacher
            {
                Address = "Jardines del Valle, 4 calle, 1 etapa",
                BirthDate = new DateTime(1993, 3, 8),
                City = "San Pedro Sula",
                Country = "Honduras",
                Disable = false,
                FirstName = "Jose",
                LastName = "Avila",
                FullName = "Jose Avila",
                IdNumber = "0501-1993-08405",
                MyGender = Gender.Masculino,
                Nationality = "Nacional",
                State = "Cortes",
                User = genericTeacher
            };
            generTeacher = _teacherRepository.Create(generTeacher);

            var academicYear = new AcademicYear { IsActive = true, Year = 2015 };
            academicYear = _academicYearRepository.Create(academicYear);
            for (int i = 1; i <= 13; i++)
            {
                var rnd = new Random();
                char section = 'A';
                do
                {
                    var grade = _gradeRepository.GetById(i);
                    var newGrade = new AcademicYearGrade
                    {
                        Grade = grade,
                        AcademicYear = academicYear,
                        Section = section++ + "",
                        ActivePensum = grade.Pensums.ElementAt(0)
                    };
                    newGrade = _academicYearGradeRepository.Create(newGrade);
                    foreach (var course in newGrade.ActivePensum.Courses)
                    {
                        var academicCourse = new AcademicYearCourse
                        {
                            Course = course,
                            AcademicYearGrade = newGrade,
                            Schedule = newGrade.CoursesDetails.Any() ? 
                            newGrade.CoursesDetails.Last().Schedule.Duration().Add(new TimeSpan(0, 40, 0)) : 
                            new TimeSpan(7, 0, 0),
                            Teacher = generTeacher //TODO: Create more teachers to randomize this a little bit more?
                        };
                        academicCourse = _academicYearCourseRepository.Create(academicCourse);
                        newGrade.CoursesDetails.Add(academicCourse);
                        newGrade = _academicYearGradeRepository.Update(newGrade);
                    }
                    academicYear.Grades.Add(newGrade);
                    _academicYearRepository.Update(academicYear);
                } while (rnd.Next(0, 2) == 0);
            }
            var genericParent = new User
            {
                DisplayName = "Padre Generico",
                Email = "parent@mhotivo.org",
                Password = "password",
                IsActive = true,
                Role = _roleRepository.Filter(x => x.Name == "Padre").FirstOrDefault()
            };
            _userRepository.Create(genericParent);
            _parentRepository.Create(new Parent { IdNumber = "1234567890", FirstName = "Padre", LastName = "Generico", FullName = "Padre Generico", Disable = false, MyGender =  Gender.Femenino, User = genericParent, BirthDate = new DateTime(1956, 11, 23)});
        }
    }
}
