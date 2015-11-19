﻿using AutoMapper;
using Mhotivo.Data.Entities;
using Mhotivo.ParentSite.Models;

namespace Mhotivo.ParentSite
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<Notification, NotificationModel>().ReverseMap();
            Mapper.CreateMap<NotificationComment, NotificationCommentsModel>().ReverseMap();
            Mapper.CreateMap<Homework, HomeworkModel>().ReverseMap();
            Mapper.CreateMap<Homework, HomeworkDateModel>().ReverseMap();
        }
    }
}