using AutoMapper;
using Mhotivo.Data.Entities;
using Mhotivo.Models;

namespace Mhotivo
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<DisplayUserModel, User>();
            Mapper.CreateMap<UserEditModel, User>();
            Mapper.CreateMap<NotificationModel, Notification>();
            Mapper.CreateMap<NotificationTypeModel, NotificationType>();
            Mapper.CreateMap<DisplayRolModel, Role>();
            Mapper.CreateMap<DisplayBenefactorModel, Benefactor>();
            Mapper.CreateMap<CreateHomeworkModel, Homework>();
            //Mapper.CreateMap<Homework, CreateHomeworkModel>();
            //Mapper.CreateMap<User, DisplayUserModel>();
            //Mapper.CreateMap<User, UserEditModel>();
            //Mapper.CreateMap<Notification, NotificationModel>();
            //Mapper.CreateMap<NotificationType, NotificationTypeModel>();
            //Mapper.CreateMap<Role, DisplayRolModel>();
            //Mapper.CreateMap<Benefactor, DisplayBenefactorModel>();
        }
    }
}