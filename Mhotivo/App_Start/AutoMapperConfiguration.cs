using AutoMapper;
using Mhotivo.Data.Entities;
using Mhotivo.Models;

namespace Mhotivo
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<DisplayUserModel, User>().ReverseMap();
            Mapper.CreateMap<UserEditModel, User>().ReverseMap();
            Mapper.CreateMap<NotificationModel, Notification>().ReverseMap();
            Mapper.CreateMap<NotificationTypeModel, NotificationType>().ReverseMap();
            Mapper.CreateMap<CreateHomeworkModel, Homework>().ReverseMap();
            //Mapper.CreateMap<Homework, CreateHomeworkModel>();
            //Mapper.CreateMap<User, DisplayUserModel>();
            //Mapper.CreateMap<User, UserEditModel>();
            //Mapper.CreateMap<Notification, NotificationModel>();
            //Mapper.CreateMap<NotificationType, NotificationTypeModel>();
            //Mapper.CreateMap<Benefactor, DisplayBenefactorModel>();
        }
    }
}