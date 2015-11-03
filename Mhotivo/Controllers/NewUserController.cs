using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Mhotivo.Data.Entities;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Models;

namespace Mhotivo.Controllers
{
    public class NewUserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public NewUserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public ActionResult Index()
        {
            var newUsers = Mapper.Map<IEnumerable<User>, IEnumerable<DisplayNewUserModel>>(_userRepository.Filter(x => x.IsUsingDefaultPassword));
            return View(newUsers);
        }

        public ActionResult Details(long id)
        {
            var toReturn = Mapper.Map<User, DisplayNewUserDefaultPasswordModel>(_userRepository.Filter(x => x.Id == id).FirstOrDefault());
            return PartialView(toReturn);
        }
    }
}