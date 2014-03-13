using EntityLibrary;
using QLKhachSanWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLKhachSanWeb.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Login()
        {
            ViewBag.trangthai = "Yes";
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            ThienService SV = new ThienService();
            int kq = SV.GetUser(model.UserName, model.Password);
            if (kq == 1)
            {
                ViewBag.trangthai = "Yes";
                User team = SV.GetUsers(model.UserName, model.Password);
                HttpContext.Session["SessionUser"] = team;
                return RedirectToAction("Index", "Home");
            }
            else
                ViewBag.trangthai = "No";
            return View();
        }
    }
}
