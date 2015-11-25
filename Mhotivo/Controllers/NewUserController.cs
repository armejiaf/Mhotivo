using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Mhotivo.Data.Entities;
using Mhotivo.Interface.Interfaces;
using Mhotivo.Models;
using PagedList;

namespace Mhotivo.Controllers
{
    public class NewUserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public NewUserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            var newUsers = _userRepository.Filter(x => x.IsUsingDefaultPassword);
            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;
            if (!string.IsNullOrEmpty(searchString))
            {
                try
                {
                    newUsers = _userRepository.Filter(x => x.IsUsingDefaultPassword && (x.UserOwner.FullName.Contains(searchString) || x.Email.Contains(searchString)));
                }
                catch (Exception)
                {
                    newUsers = _userRepository.Filter(x => x.IsUsingDefaultPassword);
                }
            }
            ViewBag.CurrentFilter = searchString;
            var model = Mapper.Map<IEnumerable<User>, IEnumerable<NewUserDisplayModel>>(newUsers.OrderBy(x => x.UserOwner.FullName));
            const int pageSize = 10;
            var pageNumber = (page ?? 1);
            return View(model.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(long id)
        {
            var toReturn = Mapper.Map<User, NewUserDefaultPasswordDisplayModel>(_userRepository.Filter(x => x.Id == id).FirstOrDefault());
            return PartialView(toReturn);
        }
    }
}