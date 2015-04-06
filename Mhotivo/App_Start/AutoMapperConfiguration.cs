using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            Mapper.CreateMap<User, DisplayUserModel>();

            Mapper.CreateMap<UserEditModel, User>();
            Mapper.CreateMap<User, UserEditModel>();

            Mapper.CreateMap<Notification, NotificationModel>();
            Mapper.CreateMap<NotificationModel, Notification>();

            Mapper.CreateMap<NotificationType, NotificationTypeModel>();
            Mapper.CreateMap<NotificationTypeModel, NotificationType>();

            Mapper.CreateMap<DisplayRolModel, Role>();
            Mapper.CreateMap<Role, DisplayRolModel>();

            Mapper.CreateMap<DisplayBenefactorModel, Benefactor>();
            Mapper.CreateMap<Benefactor, DisplayBenefactorModel>();
            
        }
    }
}