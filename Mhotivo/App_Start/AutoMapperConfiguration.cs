using AutoMapper;
using Mhotivo.Data.Entities;
using Mhotivo.Implement;
using Mhotivo.Models;

namespace Mhotivo
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<Course, CourseEditModel>().ReverseMap();
            Mapper.CreateMap<Course, CourseRegisterModel>().ReverseMap();
            Mapper.CreateMap<Course, DisplayCourseModel>().ReverseMap();
            Mapper.CreateMap<EducationLevel, DisplayEducationLevelModel>().ReverseMap();
            Mapper.CreateMap<EducationLevel, EducationLevelEditModel>().ReverseMap();
            Mapper.CreateMap<Grade, DisplayGradeModel>().ReverseMap();
            Mapper.CreateMap<Grade, GradeEditModel>().ReverseMap();
            Mapper.CreateMap<Grade, GradeRegisterModel>().ReverseMap();
            Mapper.CreateMap<Homework, CreateHomeworkModel>().ReverseMap();
            Mapper.CreateMap<Homework, DisplayHomeworkModel>().ReverseMap();
            Mapper.CreateMap<Homework, EditHomeworkModel>().ReverseMap();
            Mapper.CreateMap<Notification, NotificationModel>().ReverseMap();
            Mapper.CreateMap<NotificationType, NotificationTypeModel>().ReverseMap();
            Mapper.CreateMap<Parent, DisplayParentModel>().ForMember(p => p.Gender, o => o.MapFrom(src => src.MyGender.ToString("G"))).ReverseMap().ForMember(p => p.MyGender, o => o.MapFrom(src => Utilities.DefineGender(src.Gender)));
            Mapper.CreateMap<Parent, ParentEditModel>().ForMember(p => p.Gender, o => o.MapFrom(src => src.MyGender.ToString("G"))).ReverseMap().ForMember(p => p.MyGender, o => o.MapFrom(src => Utilities.DefineGender(src.Gender)));
            Mapper.CreateMap<Parent, ParentRegisterModel>().ForMember(p => p.Gender, o => o.MapFrom(src => src.MyGender.ToString("G"))).ReverseMap().ForMember(p => p.MyGender, o => o.MapFrom(src => Utilities.DefineGender(src.Gender)));
            Mapper.CreateMap<Pensum, DisplayPensumModel>().ReverseMap();
            Mapper.CreateMap<Student, DisplayStudentModel>().ForMember(p => p.Gender, o => o.MapFrom(src => src.MyGender.ToString("G"))).ReverseMap().ForMember(p => p.MyGender, o => o.MapFrom(src => Utilities.DefineGender(src.Gender)));
            Mapper.CreateMap<Student, StudentEditModel>().ForMember(p => p.Gender, o => o.MapFrom(src => src.MyGender.ToString("G"))).ReverseMap().ForMember(p => p.MyGender, o => o.MapFrom(src => Utilities.DefineGender(src.Gender)));
            Mapper.CreateMap<Student, StudentRegisterModel>().ForMember(p => p.Gender, o => o.MapFrom(src => src.MyGender.ToString("G"))).ReverseMap().ForMember(p => p.MyGender, o => o.MapFrom(src => Utilities.DefineGender(src.Gender)));
            Mapper.CreateMap<Teacher, DisplayTeacherModel>().ForMember(p => p.Gender, o => o.MapFrom(src => src.MyGender.ToString("G"))).ReverseMap().ForMember(p => p.MyGender, o => o.MapFrom(src => Utilities.DefineGender(src.Gender)));
            Mapper.CreateMap<Teacher, TeacherEditModel>().ForMember(p => p.Gender, o => o.MapFrom(src => src.MyGender.ToString("G"))).ReverseMap().ForMember(p => p.MyGender, o => o.MapFrom(src => Utilities.DefineGender(src.Gender)));
            Mapper.CreateMap<Teacher, TeacherRegisterModel>().ForMember(p => p.Gender, o => o.MapFrom(src => src.MyGender.ToString("G"))).ReverseMap().ForMember(p => p.MyGender, o => o.MapFrom(src => Utilities.DefineGender(src.Gender)));
            Mapper.CreateMap<User, DisplayUserModel>().ReverseMap();
            Mapper.CreateMap<User, UserEditModel>().ReverseMap();
        }
    }
}