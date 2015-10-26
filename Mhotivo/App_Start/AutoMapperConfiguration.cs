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
            Mapper.CreateMap<Parent, DisplayParentModel>().ForMember(p => p.MyGender, o => o.MapFrom(src => src.MyGender.ToString("G"))).ReverseMap().ForMember(p => p.MyGender, o => o.MapFrom(src => Utilities.DefineGender(src.MyGender)));
            Mapper.CreateMap<Parent, ParentEditModel>().ForMember(p => p.MyGender, o => o.MapFrom(src => src.MyGender.ToString("G"))).ReverseMap().ForMember(p => p.MyGender, o => o.MapFrom(src => Utilities.DefineGender(src.MyGender)));
            Mapper.CreateMap<Parent, ParentRegisterModel>().ForMember(p => p.MyGender, o => o.MapFrom(src => src.MyGender.ToString("G"))).ReverseMap().ForMember(p => p.MyGender, o => o.MapFrom(src => Utilities.DefineGender(src.MyGender)));
            Mapper.CreateMap<Pensum, DisplayPensumModel>().ReverseMap();
            Mapper.CreateMap<Student, DisplayStudentModel>().ForMember(p => p.MyGender, o => o.MapFrom(src => src.MyGender.ToString("G"))).ReverseMap().ForMember(p => p.MyGender, o => o.MapFrom(src => Utilities.DefineGender(src.MyGender)));
            Mapper.CreateMap<Student, StudentEditModel>().ForMember(p => p.MyGender, o => o.MapFrom(src => src.MyGender.ToString("G"))).ReverseMap().ForMember(p => p.MyGender, o => o.MapFrom(src => Utilities.DefineGender(src.MyGender)));
            Mapper.CreateMap<Student, StudentRegisterModel>().ForMember(p => p.MyGender, o => o.MapFrom(src => src.MyGender.ToString("G"))).ReverseMap().ForMember(p => p.MyGender, o => o.MapFrom(src => Utilities.DefineGender(src.MyGender)));
            Mapper.CreateMap<Teacher, DisplayTeacherModel>().ForMember(p => p.MyGender, o => o.MapFrom(src => src.MyGender.ToString("G"))).ReverseMap().ForMember(p => p.MyGender, o => o.MapFrom(src => Utilities.DefineGender(src.MyGender)));
            Mapper.CreateMap<Teacher, TeacherEditModel>().ForMember(p => p.MyGender, o => o.MapFrom(src => src.MyGender.ToString("G"))).ReverseMap().ForMember(p => p.MyGender, o => o.MapFrom(src => Utilities.DefineGender(src.MyGender)));
            Mapper.CreateMap<Teacher, TeacherRegisterModel>().ForMember(p => p.MyGender, o => o.MapFrom(src => src.MyGender.ToString("G"))).ReverseMap().ForMember(p => p.MyGender, o => o.MapFrom(src => Utilities.DefineGender(src.MyGender)));
            Mapper.CreateMap<User, DisplayUserModel>().ForMember(p => p.RoleName, o => o.MapFrom(src => src.Role.Name))
                .ForMember(p => p.Status, o => o.MapFrom(src => src.IsActive ? "Activo" : "No Activo")).ReverseMap();
            Mapper.CreateMap<User, DisplayNewUserModel>().ForMember(p => p.RoleName, o => o.MapFrom(src => src.Role.Name)).ReverseMap();
            Mapper.CreateMap<User, DisplayNewUserDefaultPasswordModel>().ReverseMap();
            Mapper.CreateMap<User, UserEditModel>().ReverseMap().ForMember(p => p.Role, o => o.MapFrom(src => src.RoleId));
        }
    }
}