using AutoMapper;
using Mhotivo.Data.Entities;
using Mhotivo.ParentSite.Models;

namespace Mhotivo.ParentSite
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<NotificationModel, Notification>();
            Mapper.CreateMap<NotificationCommentsModel, NotificationComments>();
            Mapper.CreateMap<PeopleModel, People>();
            Mapper.CreateMap<HomeworkModel, Homework>();
            Mapper.CreateMap<HomeworkDateModel, Homework>();
            //Mapper.CreateMap<Notification, NotificationModel>();
            //Mapper.CreateMap<NotificationComments, NotificationCommentsModel>();
            //Mapper.CreateMap<People, PeopleModel>();
            //Mapper.CreateMap<Homework, HomeworkModel>();
            //Mapper.CreateMap<Homework, HomeworkDateModel>();
        }
    }
}