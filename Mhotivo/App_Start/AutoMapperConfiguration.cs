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
            //RegisterModels: Map Model to Entity
            Mapper.CreateMap<CourseRegisterModel, Course>();
            Mapper.CreateMap<GradeRegisterModel, Grade>();
            Mapper.CreateMap<CreateHomeworkModel, Homework>();
            Mapper.CreateMap<ParentRegisterModel, Parent>().ForMember(p => p.MyGender, o => o.MapFrom(src => Utilities.DefineGender(src.MyGender)));
            Mapper.CreateMap<StudentRegisterModel, Student>().ForMember(p => p.MyGender, o => o.MapFrom(src => Utilities.DefineGender(src.MyGender)));
            Mapper.CreateMap<TeacherRegisterModel, Teacher>().ForMember(p => p.MyGender, o => o.MapFrom(src => Utilities.DefineGender(src.MyGender)));
            Mapper.CreateMap<NotificationRegisterModel, Notification>();
            //DisplayModels: Map Entity to Model
            Mapper.CreateMap<Grade, DisplayGradeModel>().ForMember(p => p.EducationLevel, o => o.MapFrom(src => src.EducationLevel.Name));
            Mapper.CreateMap<Course, DisplayCourseModel>();
            Mapper.CreateMap<Notification, NotificationDisplayModel>();
            Mapper.CreateMap<EducationLevel, DisplayEducationLevelModel>();
            Mapper.CreateMap<Homework, DisplayHomeworkModel>();
            Mapper.CreateMap<Parent, DisplayParentModel>().ForMember(p => p.MyGender, o => o.MapFrom(src => src.MyGender.ToString("G")));
            Mapper.CreateMap<Pensum, DisplayPensumModel>();
            Mapper.CreateMap<Student, DisplayStudentModel>().ForMember(p => p.MyGender, o => o.MapFrom(src => src.MyGender.ToString("G")));
            Mapper.CreateMap<Teacher, DisplayTeacherModel>().ForMember(p => p.MyGender, o => o.MapFrom(src => src.MyGender.ToString("G")));
            Mapper.CreateMap<User, DisplayUserModel>().ForMember(p => p.RoleName, o => o.MapFrom(src => src.Role.Name))
                .ForMember(p => p.Status, o => o.MapFrom(src => src.IsActive ? "Activo" : "No Activo"));
            Mapper.CreateMap<User, DisplayNewUserModel>().ForMember(p => p.RoleName, o => o.MapFrom(src => src.Role.Name));
            Mapper.CreateMap<User, DisplayNewUserDefaultPasswordModel>();
            //EditModels: Map Entity to Model and ReverseMap
            Mapper.CreateMap<Course, CourseEditModel>().ReverseMap();
            Mapper.CreateMap<EducationLevel, EducationLevelEditModel>().ReverseMap();
            Mapper.CreateMap<Grade, GradeEditModel>().ReverseMap();
            Mapper.CreateMap<Homework, EditHomeworkModel>().ReverseMap();
            Mapper.CreateMap<Parent, ParentEditModel>().ForMember(p => p.MyGender, o => o.MapFrom(src => src.MyGender.ToString("G"))).ReverseMap().ForMember(p => p.MyGender, o => o.MapFrom(src => Utilities.DefineGender(src.MyGender)));
            Mapper.CreateMap<Student, StudentEditModel>().ForMember(p => p.MyGender, o => o.MapFrom(src => src.MyGender.ToString("G"))).ReverseMap().ForMember(p => p.MyGender, o => o.MapFrom(src => Utilities.DefineGender(src.MyGender)));
            Mapper.CreateMap<Teacher, TeacherEditModel>().ForMember(p => p.MyGender, o => o.MapFrom(src => src.MyGender.ToString("G"))).ReverseMap().ForMember(p => p.MyGender, o => o.MapFrom(src => Utilities.DefineGender(src.MyGender)));
            Mapper.CreateMap<User, UserEditModel>().ReverseMap().ForMember(p => p.Role, o => o.MapFrom(src => src.RoleId));
        }
    }
}