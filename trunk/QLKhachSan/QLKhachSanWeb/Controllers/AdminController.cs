using QLKhachSanWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityLibrary;

namespace QLKhachSanWeb.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        ThienService _service = new ThienService();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddUsers()
        {

            return View();
        }
        [HttpPost]
        public ActionResult AddUsers(UserModel model)
        {

            User NewUser = new User();
            NewUser.Name = model.Name;
            NewUser.UserName = model.UserName;
            NewUser.Password = model.Password;
            NewUser.IsActive = true;
            NewUser.PermissonId =Convert.ToInt16(model.PermissonId);
            NewUser.LastUpdateDate = DateTime.Now;
            NewUser.CreateDate = DateTime.Now;

            int i = _service.InsertUser(NewUser);
            if (i == 1)
                return Json("success");
            return Json("fail");
        }


        //List users

        public ActionResult ListUsers()
        {
            var query = _service.GetListUsers();

            List<UserModel> ListUserModel = new List<UserModel>();

            foreach (User us in query)
            {
                UserModel team = new UserModel();
                team.Id = us.Id;
                team.Name = us.Name;
                team.UserName = us.UserName;
                team.Password = us.Password;
                team.IsActive = 1;
                team.PermissonId = us.PermissonId;

                ListUserModel.Add(team);
            }
            return View(ListUserModel);
        }


        public ActionResult UpdateUser(int id)
        {
            User us = _service.GetUsersByID(id);
            UserModel model = new UserModel();
            model.Id = us.Id;
            model.Name = us.Name;
            model.UserName = us.UserName;
            model.Password = us.Password;
            model.PermissonId = us.PermissonId;
            model.CreateDate = us.CreateDate.ToString();
            model.IsActive = 1;
            return View(model);
        }

        [HttpPost]

        public ActionResult UpdateUser(UserModel model)
        {
            User us = new User();
            us.Id = model.Id;
            us.Name = model.Name;
            us.UserName = model.UserName;
            us.Password = model.Password;
            us.PermissonId =Convert.ToInt16( model.PermissonId);
            us.CreateDate = DateTime.Now;
                //model.CreateDate;
            us.LastUpdateDate = DateTime.Now;
            if (model.IsActive == 1)
                us.IsActive = true;
            else
                us.IsActive = false;
            int kq = _service.UpdateUser(us);
            if(kq==1)
                return RedirectToAction("Index", "Home");
            return View();
        }
    }
}
