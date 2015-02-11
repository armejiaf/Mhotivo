using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Providers.Entities;
using AutoMapper;
using Mhotivo.Data.Entities;
using Mhotivo.ParentSite.Models;

namespace Mhotivo.ParentSite
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<Notification, NotificationModel>();
            Mapper.CreateMap<NotificationModel, Notification>();
        }
    }
}