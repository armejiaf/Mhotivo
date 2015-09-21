using AutoMapper;
using Mhotivo.Data.Entities;
using Mhotivo.ParentSite.Models;

namespace Mhotivo.ParentSite
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<NotificationModel, Notification>().ReverseMap();
            Mapper.CreateMap<NotificationCommentsModel, NotificationComments>().ReverseMap();
            Mapper.CreateMap<HomeworkModel, Homework>().ReverseMap();
            Mapper.CreateMap<HomeworkDateModel, Homework>().ReverseMap();
            //Mapper.CreateMap<Notification, NotificationModel>();
            //Mapper.CreateMap<NotificationComments, NotificationCommentsModel>();
            //Mapper.CreateMap<People, PeopleModel>();
            //Mapper.CreateMap<Homework, HomeworkModel>();
            //Mapper.CreateMap<Homework, HomeworkDateModel>();
        }
    }
}