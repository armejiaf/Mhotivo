using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Mhotivo.Data.Entities;
using Mhotivo.Implement.Utils;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Models;

namespace Mhotivo
{
    public class AutoMapperConfiguration
    {

        public static void Configure()
        {
            MapAcademicYearModels();
            MapAcademicGradeModels();
            MapAcademicCourseModels();
            MapContactInformationModels();
            MapCourseModels();
            MapEducationLevelModels();
            MapEnrollModels();
            MapGradeModels();
            MapHomeworkModels();
            MapNotificationModels();
            MapNotificationCommentModels();
            MapPensumModels();
            MapUserModels();
            MapTutorModels();
            MapStudentModels();
            MapTeacherModels();
            MapAdministrativeModels();
        }

        private static void MapAcademicYearModels()
        {
            Mapper.CreateMap<AcademicYearRegisterModel, AcademicYear>();
            Mapper.CreateMap<AcademicYear, AcademicYearDisplayModel>();
            Mapper.CreateMap<AcademicYear, AcademicYearEditModel>().ReverseMap();
        }

        private static void MapAcademicGradeModels()
        {
            Mapper.CreateMap<AcademicGradeRegisterModel, AcademicGrade>()
                .ForMember(g => g.AcademicYear,
                    o => o.MapFrom(src => ((IAcademicYearRepository) DependencyResolver.Current.GetService(
                        typeof(IAcademicYearRepository))).GetById(src.AcademicYear)))
                .ForMember(g => g.Grade,
                    o => o.MapFrom(src => ((IGradeRepository) DependencyResolver.Current.GetService(
                        typeof(IGradeRepository))).GetById(src.Grade)))
                .ForMember(g => g.ActivePensum,
                    o => o.MapFrom(src => ((IPensumRepository) DependencyResolver.Current.GetService(
                        typeof(IPensumRepository))) .GetById(src.ActivePensum)));
            Mapper.CreateMap<AcademicGrade, AcademicGradeDisplayModel>()
                .ForMember(p => p.ActivePensum, o => o.MapFrom(src => src.ActivePensum.Name))
                .ForMember(p => p.Grade, o => o.MapFrom(src => src.Grade.Name))
                .ForMember(p => p.SectionTeacher,
                    o => o.MapFrom(src => src.SectionTeacher != null ? src.SectionTeacher.FullName : "Maestro No Asignado"));
            Mapper.CreateMap<AcademicGrade, AcademicGradeEditModel>()
                .ForMember(p => p.ActivePensum, o => o.MapFrom(src => src.ActivePensum.Id))
                .ForMember(p => p.Grade, o => o.MapFrom(src => src.Grade.Id))
                .ReverseMap()
                .ForMember(g => g.ActivePensum,
                    o => o.MapFrom(src => ((IPensumRepository) DependencyResolver.Current.GetService(
                        typeof (IPensumRepository))).GetById(src.ActivePensum)))
                .ForMember(g => g.Grade,
                    o => o.MapFrom(src => ((IGradeRepository) DependencyResolver.Current.GetService(
                        typeof (IGradeRepository))).GetById(src.Grade)));
            Mapper.CreateMap<AcademicGrade, AcademicGradeTeacherAssignModel>()
                .ForMember(p => p.SectionTeacher,
                    o => o.MapFrom(src => src.SectionTeacher != null ? src.SectionTeacher.Id : -1))
                .ReverseMap()
                .ForMember(g => g.SectionTeacher,
                    o => o.MapFrom(src => src.SectionTeacher != -1
                        ? ((ITeacherRepository)DependencyResolver.Current.GetService(
                        typeof(ITeacherRepository))).GetById(src.SectionTeacher) : null));

        }

        private static void MapAcademicCourseModels()
        {
            Mapper.CreateMap<AcademicCourse, AcademicCourseDisplayModel>()
                .ForMember(p => p.Course, o => o.MapFrom(src => src.Course.Name))
                .ForMember(p => p.Teacher, o => o.MapFrom(src => src.Teacher.FullName))
                .ForMember(p => p.Schedule, o => o.MapFrom(src => src.Schedule.ToAmPm()));
            Mapper.CreateMap<AcademicCourse, AcademicCourseEditModel>()
                .ForMember(p => p.Teacher, o => o.MapFrom(src => src.Teacher.Id))
                .ReverseMap()
                .ForMember(g => g.Teacher,
                    o => o.MapFrom(src => ((ITeacherRepository) DependencyResolver.Current.GetService(
                        typeof (ITeacherRepository))).GetById(src.Teacher)));
        }

        private static void MapContactInformationModels()
        {
            Mapper.CreateMap<ContactInformationRegisterModel, ContactInformation>()
                .ForMember(p => p.People,
                    o => o.MapFrom( src => ((IPeopleRepository) DependencyResolver.Current.GetService(
                        typeof (IPeopleRepository))).GetById(src.People)));
            Mapper.CreateMap<ContactInformation, ContactInformationEditModel>().ReverseMap();
        }

        private static void MapCourseModels()
        {
            Mapper.CreateMap<CourseRegisterModel, Course>()
                .ForMember(g => g.Pensum,
                    o => o.MapFrom(src => ((IPensumRepository) DependencyResolver.Current.GetService(
                        typeof (IPensumRepository))).GetById(src.Pensum)))
                .ReverseMap()
                .ForMember(p => p.Pensum, o => o.MapFrom(src => src.Pensum.Id));
            Mapper.CreateMap<Course, CourseDisplayModel>();
            Mapper.CreateMap<Course, CourseEditModel>().ReverseMap();
        }

        private static void MapEducationLevelModels()
        {
            Mapper.CreateMap<EducationLevelRegisterModel, EducationLevel>();
            Mapper.CreateMap<EducationLevel, EducationLevelDisplayModel>()
                .ForMember(p => p.Director, o => o.MapFrom(src => src.Director != null ? src.Director.UserOwner.FullName : "Director No Asignado"));
            Mapper.CreateMap<EducationLevel, EducationLevelEditModel>().ReverseMap();
            Mapper.CreateMap<EducationLevel, EducationLevelDirectorAssignModel>()
                .ForMember(p => p.Director, o => o.MapFrom(src => src.Director.Id))
                .ReverseMap()
                .ForMember(g => g.Director,
                    o => o.MapFrom( src => ((IUserRepository) DependencyResolver.Current.GetService(
                        typeof(IUserRepository))).GetById(src.Director)));
        }

        private static void MapEnrollModels()
        {
           // Mapper.CreateMap<AcademicGrade, IEnumerable<EnrollDisplayModel>>();
        }

        private static void MapGradeModels()
        {
            Mapper.CreateMap<GradeRegisterModel, Grade>()
                .ForMember(p => p.EducationLevel,
                    o => o.MapFrom(src => ((IEducationLevelRepository) DependencyResolver.Current.GetService(
                        typeof (IEducationLevelRepository))).GetById(src.EducationLevel)));
            Mapper.CreateMap<Grade, GradeDisplayModel>()
                .ForMember(p => p.EducationLevel, o => o.MapFrom(src => src.EducationLevel.Name));
            Mapper.CreateMap<Grade, GradeEditModel>()
                .ForMember(p => p.EducationLevel, o => o.MapFrom(src => src.EducationLevel.Id))
                .ReverseMap()
                .ForMember(p => p.EducationLevel,
                    o => o.MapFrom(src => ((IEducationLevelRepository)DependencyResolver.Current.GetService(
                        typeof(IEducationLevelRepository))).GetById(src.EducationLevel)));
        }

        private static void MapHomeworkModels()
        {
            Mapper.CreateMap<HomeworkRegisterModel, Homework>()
                .ForMember(p => p.AcademicCourse,
                    o => o.MapFrom(src => ((IAcademicCourseRepository)DependencyResolver.Current.GetService(
                        typeof(IAcademicCourseRepository))).GetById(src.AcademicCourse)))
                .ForMember(p => p.DeliverDate, o => o.MapFrom(src => new DateTime(src.Year, src.Month, src.Day, src.Hour.Hours, src.Hour.Minutes, src.Hour.Seconds)));
            Mapper.CreateMap<Homework, HomeworkDisplayModel>()
                .ForMember(p => p.DeliverDate, o => o.MapFrom(src => src.DeliverDate))
                .ForMember(p => p.AcademicCourse, o => o.MapFrom(src => src.AcademicCourse.Course.Name));
            Mapper.CreateMap<Homework, HomeworkEditModel>()
                .ForMember(p => p.Year, o => o.MapFrom(src => src.DeliverDate.Year))
                .ForMember(p => p.Month, o => o.MapFrom(src => src.DeliverDate.Month))
                .ForMember(p => p.Day, o => o.MapFrom(src => src.DeliverDate.Day))
                .ForMember(p => p.Hour, o => o.MapFrom(src => src.DeliverDate.TimeOfDay))
                .ReverseMap()
                .ForMember(p => p.DeliverDate, o => o.MapFrom(src => new DateTime(src.Year, src.Month, src.Day, src.Hour.Hours, src.Hour.Minutes, src.Hour.Seconds)));
        }

        private static void MapNotificationModels()
        {
            Mapper.CreateMap<NotificationRegisterModel, Notification>()
                .ForMember(p => p.NotificationCreator,
                o => o.MapFrom(src => ((IPeopleWithUserRepository)DependencyResolver.Current.GetService(
                        typeof(IPeopleWithUserRepository))).GetById(src.NotificationCreator)))
                .ForMember(p => p.AcademicYear,
                o => o.MapFrom(src => ((IAcademicYearRepository)DependencyResolver.Current.GetService(
                        typeof(IAcademicYearRepository))).GetById(src.AcademicYear)));
            Mapper.CreateMap<Notification, NotificationDisplayModel>()
                .ForMember(p => p.NotificationType, o => o.MapFrom(src => src.NotificationType.GetEnumDescription()))
                .ForMember(p => p.NotificationCreator,
                    o => o.MapFrom(src => src.NotificationCreator.FirstName))
                .ForMember(p => p.CreationDate, o => o.MapFrom(src => src.CreationDate.ToString()))
                .ForMember(p => p.DestinationId, o => o.MapFrom(src =>
                    src.NotificationType == NotificationType.General
                        ? "General"
                    : src.NotificationType == NotificationType.EducationLevel
                        ? ((IEducationLevelRepository) DependencyResolver.Current.GetService(
                            typeof (IEducationLevelRepository))).GetById(src.DestinationId).Name
                    : src.NotificationType == NotificationType.Grade
                        ? ((IGradeRepository) DependencyResolver.Current.GetService(
                            typeof (IGradeRepository))).GetById(src.DestinationId).Name
                    : src.NotificationType == NotificationType.Section
                        ? ((IAcademicGradeRepository) DependencyResolver.Current.GetService(
                            typeof (IAcademicGradeRepository))).GetById(src.DestinationId).Grade.Name + " " +
                        ((IAcademicGradeRepository) DependencyResolver.Current.GetService(
                          typeof (IAcademicGradeRepository))).GetById(src.DestinationId).Section
                    : src.NotificationType == NotificationType.Course
                        ? ((ICourseRepository) DependencyResolver.Current.GetService(
                            typeof (ICourseRepository))).GetById(src.DestinationId).Name
                    : src.NotificationType == NotificationType.Personal
                        ? ((IStudentRepository) DependencyResolver.Current.GetService(
                            typeof (IStudentRepository))).GetById(src.DestinationId).FullName
                        : ""));
            Mapper.CreateMap<Notification, NotificationEditModel>()
                .ForMember(p => p.Id1, o => o.MapFrom(src =>
                    src.NotificationType == NotificationType.Section || 
                    src.NotificationType == NotificationType.Course || 
                    src.NotificationType == NotificationType.Personal
                    ? ((IGradeRepository)DependencyResolver.Current.GetService(
                            typeof(IGradeRepository))).GetById(src.DestinationId).Id : -1))
                .ForMember(p => p.Id2, o => o.MapFrom(src =>
                    src.NotificationType == NotificationType.Course ||
                    src.NotificationType == NotificationType.Personal
                    ? ((IAcademicGradeRepository)DependencyResolver.Current.GetService(
                            typeof(IAcademicGradeRepository))).GetById(src.DestinationId).Id : -1))
                .ReverseMap();
        }

        private static void MapNotificationCommentModels()
        {
            Mapper.CreateMap<NotificationCommentRegisterModel, NotificationComment>()
                .ForMember(p => p.Commenter,
                    o => o.MapFrom(src => ((IUserRepository) DependencyResolver.Current.GetService(
                        typeof (IUserRepository))).GetById(src.Commenter)))
                .ForMember(p => p.Notification,
                    o => o.MapFrom(src => ((INotificationRepository) DependencyResolver.Current.GetService(
                        typeof (INotificationRepository))).GetById(src.Notification)));
            Mapper.CreateMap<NotificationComment, NotificationCommentDisplayModel>()
                .ForMember(p => p.Commenter, o => o.MapFrom(src => src.Commenter.UserOwner.FullName))
                .ForMember(p => p.CreationDate, o => o.MapFrom(src => src.CreationDate.ToString()));
            Mapper.CreateMap<NotificationComment, NotificationCommentEditModel>().ReverseMap();
        }

        private static void MapPensumModels()
        {
            Mapper.CreateMap<PensumRegisterModel, Pensum>()
                .ForMember(p => p.Grade,
                    o => o.MapFrom(src => ((IGradeRepository) DependencyResolver.Current.GetService(
                        typeof (IGradeRepository))).GetById(src.Grade)));
            Mapper.CreateMap<Pensum, PensumDisplayModel>();
            Mapper.CreateMap<Pensum, PensumEditModel>().ReverseMap();
            Mapper.CreateMap<Pensum, PensumCourseModel>()
                .ForMember(p => p.Courses, o => o.MapFrom(src => src.Courses.Select(Mapper.Map<CourseRegisterModel>)))
                .ReverseMap()
                .ForMember(p => p.Courses, o => o.MapFrom(src => src.Courses.Select(Mapper.Map<Course>)));
        }

        private static void MapUserModels()
        {
            Mapper.CreateMap<User, UserDisplayModel>()
                .ForMember(p => p.UserOwner, o => o.MapFrom(src => src.UserOwner.FullName))
                .ForMember(p => p.Role, o => o.MapFrom(src => src.Role.Name));
            Mapper.CreateMap<User, NewUserDisplayModel>()
                .ForMember(p => p.UserOwner, o => o.MapFrom(src => src.UserOwner.FullName))
                .ForMember(p => p.Role, o => o.MapFrom(src => src.Role.Name))
                .ForMember(p => p.Email, o => o.MapFrom(src => string.IsNullOrWhiteSpace(src.Email) ? src.UserOwner.IdNumber : src.Email));
            Mapper.CreateMap<User, NewUserDefaultPasswordDisplayModel>()
                .ForMember(p => p.UserOwner, o => o.MapFrom(src => src.UserOwner.FullName));
            Mapper.CreateMap<User, UserEditModel>()
                .ForMember(p => p.Role, o => o.MapFrom(src => src.Role.Id))
                .ReverseMap()
                .ForMember(p => p.Role,
                    o => o.MapFrom(src => ((IRoleRepository)DependencyResolver.Current.GetService(
                        typeof(IRoleRepository))).GetById(src.Role)));
        }

        //TODO: Not done with the ones below just yet.
        private static void MapTutorModels()
        {
            Mapper.CreateMap<TutorRegisterModel, Tutor>()
                .ForMember(p => p.FullName, o => o.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(p => p.BirthDate, o => o.MapFrom(src => new DateTime(src.Year, src.Month, src.Day)));
            Mapper.CreateMap<Tutor, TutorDisplayModel>()
                .ForMember(p => p.BirthDate, o => o.MapFrom(src => src.BirthDate.ToString("MMMM dd, yyyy", new CultureInfo("es-HN"))));
            Mapper.CreateMap<Tutor, TutorEditModel>()
                .ForMember(p => p.Year, o => o.MapFrom(src => src.BirthDate.Year))
                .ForMember(p => p.Month, o => o.MapFrom(src => src.BirthDate.Month))
                .ForMember(p => p.Day, o => o.MapFrom(src => src.BirthDate.Day))
                .ReverseMap()
                .ForMember(p => p.FullName, o => o.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(p => p.BirthDate, o => o.MapFrom(src => new DateTime(src.Year, src.Month, src.Day)));
        }

        private static void MapStudentModels()
        {
            Mapper.CreateMap<StudentRegisterModel, Student>()
                .ForMember(p => p.FullName, o => o.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(p => p.Tutor1, o => o.MapFrom(src => ((ITutorRepository)DependencyResolver.Current.GetService(
                        typeof(ITutorRepository))).GetById(src.Tutor1)))
                .ForMember(p => p.BirthDate, o => o.MapFrom(src => new DateTime(src.Year, src.Month, src.Day)));
            Mapper.CreateMap<Student, StudentDisplayModel>()
                .ForMember(p => p.Tutor1, o => o.MapFrom(src => src.Tutor1.FullName))
                .ForMember(p => p.BirthDate, o => o.MapFrom(src => src.BirthDate.ToString("MMMM dd, yyyy", new CultureInfo("es-HN"))));
            Mapper.CreateMap<Student, StudentEditModel>()
                .ForMember(p => p.Year, o => o.MapFrom(src => src.BirthDate.Year))
                .ForMember(p => p.Month, o => o.MapFrom(src => src.BirthDate.Month))
                .ForMember(p => p.Day, o => o.MapFrom(src => src.BirthDate.Day))
                .ReverseMap()
                .ForMember(p => p.FullName, o => o.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(p => p.Tutor1, o => o.MapFrom(src => ((ITutorRepository)DependencyResolver.Current.GetService(
                        typeof(ITutorRepository))).GetById(src.Tutor1)))
                .ForMember(p => p.BirthDate, o => o.MapFrom(src => new DateTime(src.Year, src.Month, src.Day)));
        }

        private static void MapTeacherModels()
        {
            Mapper.CreateMap<TeacherRegisterModel, Teacher>()
                .ForMember(p => p.FullName, o => o.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(p => p.BirthDate, o => o.MapFrom(src => new DateTime(src.Year, src.Month, src.Day)));
            Mapper.CreateMap<Teacher, TeacherDisplayModel>()
                .ForMember(p => p.BirthDate, o => o.MapFrom(src => src.BirthDate.ToString("MMMM dd, yyyy", new CultureInfo("es-HN"))));
            Mapper.CreateMap<Teacher, TeacherEditModel>()
                .ForMember(p => p.Year, o => o.MapFrom(src => src.BirthDate.Year))
                .ForMember(p => p.Month, o => o.MapFrom(src => src.BirthDate.Month))
                .ForMember(p => p.Day, o => o.MapFrom(src => src.BirthDate.Day))
                .ReverseMap()
                .ForMember(p => p.FullName, o => o.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(p => p.BirthDate, o => o.MapFrom(src => new DateTime(src.Year, src.Month, src.Day)));
        }

        private static void MapAdministrativeModels()
        {
            Mapper.CreateMap<AdministrativeRegisterModel, PeopleWithUser>()
                .ForMember(p => p.FullName, o => o.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(p => p.BirthDate, o => o.MapFrom(src => new DateTime(src.Year, src.Month, src.Day)));
            Mapper.CreateMap<PeopleWithUser, AdministrativeDisplayModel>()
                .ForMember(p => p.Role, o => o.MapFrom(src => src.User.Role.Name))
                .ForMember(p => p.BirthDate, o => o.MapFrom(src => src.BirthDate.ToString("MMMM dd, yyyy", new CultureInfo("es-HN"))));
            Mapper.CreateMap<PeopleWithUser, AdministrativeEditModel>()
                .ForMember(p => p.Year, o => o.MapFrom(src => src.BirthDate.Year))
                .ForMember(p => p.Month, o => o.MapFrom(src => src.BirthDate.Month))
                .ForMember(p => p.Day, o => o.MapFrom(src => src.BirthDate.Day))
                .ReverseMap()
                .ForMember(p => p.FullName, o => o.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(p => p.BirthDate, o => o.MapFrom(src => new DateTime(src.Year, src.Month, src.Day)));
        }
    }
}