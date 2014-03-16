using QLKhachSanWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityLibrary;
using QLKhachSanWeb.Helper;


namespace QLKhachSanWeb.Controllers
{
     [Auth]
    public class AdminController : Controller
    {
        

        #region Trang cu

        ThienGetService123 _service = new ThienGetService123();

        User KTQUyen()
        {
            User team = new User();
            team = null;
            if (this.Session["SessionUser"] != null)
            {
                team = (User)this.Session["SessionUser"];
            }
            return team;
        }

        public ActionResult QuyenTruyCap(User team)
        {
            return View();
        }

        public ActionResult Index()
        {
            User team = KTQUyen();
            if (team == null)
                return RedirectToAction("Login", "Login");
            else
            {
                if (team.PermissonId == 2)
                    return RedirectToAction("QuyenTruyCap", "Admin");
                else
                    return RedirectToAction("XemLog", "Admin");
            }
        }

        #endregion

        //Users
        #region Users

        public ActionResult AddUsers()
        {

            User team = KTQUyen();
            if (team == null)
                return RedirectToAction("Login", "Login");
            else
            {
                if (team.PermissonId == 2)
                    return RedirectToAction("QuyenTruyCap", "Admin");
                else
                    return View();
            }
        }
        [HttpPost]
        public ActionResult AddUsers(UserModel model)
        {
            try
            {
                User NewUser = new User();
                NewUser.Name = model.Name;
                NewUser.UserName = model.UserName;
                NewUser.Password = model.Password;
                NewUser.IsActive = true;
                NewUser.PermissonId = Convert.ToInt16(model.PermissonId);
                NewUser.LastUpdateDate = DateTime.Now;
                NewUser.CreateDate = DateTime.Now;

                int kq = _service.InsertUser(NewUser);
                if (kq == 1)
                {
                    User team = KTQUyen();
                    string tile = team.UserName + " đã thêm tài khoản " + model.UserName + " vào hệ thống";
                    _service.WriteLogAction(tile, team.Id);
                    return RedirectToAction("ListUsers", "Admin");
                }
                return View();
            }
            catch (Exception e)
            {
                return View();
            }
        }

        public ActionResult ListUsers()
        {
            User team1 = KTQUyen();
            if (team1 == null)
                return RedirectToAction("Login", "Login");
            else
            {
                if (team1.PermissonId == 2)
                    return RedirectToAction("QuyenTruyCap", "Admin");
                else
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

            }

        }


        public ActionResult UpdateUser(int id)
        {
            User team = KTQUyen();
            if (team == null)
                return RedirectToAction("Login", "Login");
            else
            {
                if (team.PermissonId == 2)
                    return RedirectToAction("QuyenTruyCap", "Admin");
                else
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
            }
        }

        [HttpPost]

        public ActionResult UpdateUser(UserModel model)
        {
            try
            {
                User us = new User();
                us.Id = model.Id;
                us.Name = model.Name;
                us.UserName = model.UserName;
                us.Password = _service.GetUsersByID(us.Id).Password;
                us.PermissonId = Convert.ToInt16(model.PermissonId);
                us.CreateDate = _service.GetUsersByID(us.Id).CreateDate;
                //model.CreateDate;
                us.LastUpdateDate = DateTime.Now;
                if (model.IsActive == 1)
                    us.IsActive = true;
                else
                    us.IsActive = false;
                int kq = _service.UpdateUser(us);
                if (kq == 1)
                {
                    User team = KTQUyen();
                    string tile = team.UserName + " đã cập nhật tài khoản " + model.UserName;
                    _service.WriteLogAction(tile, team.Id);
                    return RedirectToAction("ListUsers", "Admin");
                }
                return View();
            }
            catch (Exception e)
            {
                return View();
            }
        }



        #endregion


        //Logs
        #region Xem Logs

        public ActionResult XemLog(int? page)
        {
            int size = Convert.ToInt32(page);
            User team1 = KTQUyen();
            if (team1 == null)
                return RedirectToAction("Login", "Login");
            else
            {
                if (team1.PermissonId == 2)
                    return RedirectToAction("QuyenTruyCap", "Admin");
                else
                {
                    List<LogModel> model = new List<LogModel>();
                    var listLog = _service.GetListLog();
                    foreach (Log lg in listLog)
                    {
                        LogModel team = new LogModel();
                        team.id = lg.Id;
                        team.ActionName = lg.ActionName;
                        team.UserId = lg.UserId;
                        team.UserName = _service.GetUsersByID(lg.UserId).UserName;
                        team.Name = _service.GetUsersByID(lg.UserId).Name;
                        team.CreateDate = lg.CreatedDate.ToString();
                        model.Add(team);
                    }

                    ListLogModel modelview = new ListLogModel()
                    {
                        ListLogs = model
                        .Skip((size - 1) * 25)
                        .Take(25),
                        PageInfo = new PageInfo
                        {
                            CurrentPage = size,
                            ItemsPerPage = 25,
                            TotalItems = model.Count()
                        }
                    };

                    return View(modelview);
                }
            }
        }

        #endregion

        //dich vu
        #region Service
        public ActionResult ListService()
        {
            User team1 = KTQUyen();
            if (team1 == null)
                return RedirectToAction("Login", "Login");
            else
            {
                if (team1.PermissonId == 2)
                    return RedirectToAction("QuyenTruyCap", "Admin");
                else
                {
                    var query = _service.GetListSevice(false);
                    List<ServiceModel> model = new List<ServiceModel>();
                    foreach (Service sv in query)
                    {
                        ServiceModel gt = new ServiceModel();
                        gt.Id = sv.Id;
                        gt.Name = sv.Name;
                        gt.Price = sv.Price.ToString();
                        gt.Description = sv.Description;
                        if (sv.IsActive == true)
                            gt.IsActive = 1;
                        else
                            gt.IsActive = 2;
                        model.Add(gt);
                    }
                    return View(model);
                }
            }
        }
        public ActionResult AddNewSevice()
        {

            User team1 = KTQUyen();
            if (team1 == null)
                return RedirectToAction("Login", "Login");
            else
            {
                if (team1.PermissonId == 2)
                    return RedirectToAction("QuyenTruyCap", "Admin");
                else
                {
                    return View();
                }
            }
        }
        [HttpPost]
        public ActionResult AddNewSevice(ServiceModel model)
        {
            try
            {
                Service team = new Service();
                team.Name = model.Name;
                team.Price = decimal.Parse(model.Price);
                team.Description = model.Description;
                team.Deleted = false;
                team.IsActive = true;
                team.IsRoom = false;
                team.ServiceTypeId = 0;
                int kq = _service.InsertService(team);
                if (kq == 1)
                {
                    User team1 = KTQUyen();
                    string tile = team1.UserName + " đã thêm dịch vụ " + model.Name;
                    _service.WriteLogAction(tile, team1.Id);
                    return RedirectToAction("ListService", "Admin");
                }
                else
                    return View();
            }
            catch (Exception e)
            {
                return View();
            }
        }

        public ActionResult DeleteService(int Id)
        {
            Service model = _service.GetServiceById(Id);
            model.Deleted = true;
            int kq = _service.UpdateService(model);
            User team1 = KTQUyen();
            string tile = team1.UserName + " đã xoá dịch vụ " + model.Name;
            _service.WriteLogAction(tile, team1.Id);
            return RedirectToAction("ListService", "Admin");
        }

        public ActionResult UpdateService(int Id)
        {
            User team1 = KTQUyen();
            if (team1 == null)
                return RedirectToAction("Login", "Login");
            else
            {
                if (team1.PermissonId == 2)
                    return RedirectToAction("QuyenTruyCap", "Admin");
                else
                {
                    Service kq = _service.GetServiceById(Id);
                    ServiceModel gt = new ServiceModel();
                    gt.Id = kq.Id;
                    gt.Name = kq.Name;
                    gt.Price = kq.Price.ToString();
                    gt.Description = kq.Description;
                    if (kq.IsActive == true)
                        gt.IsActive = 1;
                    else
                        gt.IsActive = 2;
                    return View(gt);
                }
            }
        }
        [HttpPost]
        public ActionResult UpdateService(ServiceModel model)
        {
            try
            {
                Service kq = new Service();
                kq.Id = model.Id;
                kq.Name = model.Name;
                kq.Price = decimal.Parse(model.Price);
                kq.Description = model.Description;
                if (model.IsActive == 1)
                    kq.IsActive = true;
                else
                    kq.IsActive = false;
                kq.ServiceTypeId = 0;
                kq.Deleted = false;
                kq.IsRoom = false;
                int gt = _service.UpdateService(kq);
                if (gt == 1)
                {
                    User team1 = KTQUyen();
                    string tile = team1.UserName + " đã cập nhật dịch vụ " + model.Name;
                    _service.WriteLogAction(tile, team1.Id);
                    return RedirectToAction("ListService", "Admin");
                }
                else
                    return View();
            }
            catch (Exception e) 
            {
                return View();
            }
        }
        #endregion

        //Service phong khach san 
        #region ServicePhong

        public ActionResult DeleteServiceRoom(int Id)
        {
            Service model = _service.GetServiceById(Id);
            string name = model.Name;
            model.Deleted = true;
            int kq = _service.UpdateService(model);
            User team1 = KTQUyen();
            string tile = team1.UserName + " đã xoá phòng " + name;
            _service.WriteLogAction(tile, team1.Id);
            return RedirectToAction("ListServiceRoom", "Admin");
        }

        public ActionResult ListServiceRoom()
        {
            User team1 = KTQUyen();
            if (team1 == null)
                return RedirectToAction("Login", "Login");
            else
            {
                if (team1.PermissonId == 2)
                    return RedirectToAction("QuyenTruyCap", "Admin");
                else
                {
                    var query = _service.GetListSevice(true);
                    List<ServiceModel> model = new List<ServiceModel>();
                    foreach (Service sv in query)
                    {
                        ServiceModel gt = new ServiceModel();
                        gt.Id = sv.Id;
                        gt.Name = sv.Name;
                        gt.Price = sv.Price.ToString();
                        gt.Description = sv.Description;
                        gt.FloorNumber = sv.FloorNumber;
                        gt.ServiceTypeId = sv.ServiceTypeId;
                        gt.ServiceType = _service.GetServiceTypeById(sv.ServiceTypeId).Name;
                        if (sv.IsActive == true)
                            gt.IsActive = 1;
                        else
                            gt.IsActive = 2;
                        model.Add(gt);
                    }
                    return View(model);
                }
            }
        }


        public ActionResult AddNewSeviceRoom()
        {

            User team1 = KTQUyen();
            if (team1 == null)
                return RedirectToAction("Login", "Login");
            else
            {
                if (team1.PermissonId == 2)
                    return RedirectToAction("QuyenTruyCap", "Admin");
                else
                {
                    var mylist = new List<SelectListItem>();
                    var qr = _service.GetListServiceType();
                    foreach (ServiceType lv in qr)
                    {
                        mylist.Add(new SelectListItem { Text = lv.Name, Value = lv.Id.ToString() });
                    }
                    ViewBag.ListTypeService = mylist;

                    return View();
                }
            }
        }
        [HttpPost]
        public ActionResult AddNewSeviceRoom(ServiceModel model)
        {
            try
            {
                Service team = new Service();
                team.Name = model.Name;
                team.Price = decimal.Parse(model.Price);
                team.Description = model.Description;
                team.Deleted = false;
                team.IsActive = true;
                team.IsRoom = true;
                team.FloorNumber = model.FloorNumber;
                team.ServiceTypeId = model.ServiceTypeId;
                int kq = _service.InsertService(team);
                if (kq == 1)
                {
                    User team1 = KTQUyen();
                    string tile = team1.UserName + " đã thêm phòng " + model.Name;
                    _service.WriteLogAction(tile, team1.Id);
                    return RedirectToAction("ListServiceRoom", "Admin");
                }
                else
                    return View();
            }
            catch (Exception e)
            {
                return View();
            }
        }


        public ActionResult UpdateServiceRoom(int Id)
        {
            User team1 = KTQUyen();
            if (team1 == null)
                return RedirectToAction("Login", "Login");
            else
            {
                if (team1.PermissonId == 2)
                    return RedirectToAction("QuyenTruyCap", "Admin");
                else
                {
                    Service kq = _service.GetServiceById(Id);
                    ServiceModel gt = new ServiceModel();
                    gt.Id = kq.Id;
                    gt.Name = kq.Name;
                    gt.FloorNumber = kq.FloorNumber;
                    gt.ServiceTypeId = kq.ServiceTypeId;
                    gt.Price = kq.Price.ToString();
                    gt.Description = kq.Description;
                    if (kq.IsActive == true)
                        gt.IsActive = 1;
                    else
                        gt.IsActive = 2;

                    var mylist = new List<SelectListItem>();
                    var qr = _service.GetListServiceType();
                    foreach (ServiceType lv in qr)
                    {
                        mylist.Add(new SelectListItem { Text = lv.Name, Value = lv.Id.ToString() });
                    }
                    ViewBag.ListTypeService = mylist;


                    return View(gt);
                }
            }
        }
        [HttpPost]
        public ActionResult UpdateServiceRoom(ServiceModel model)
        {
            try
            {
                Service kq = new Service();
                kq.Id = model.Id;
                kq.Name = model.Name;
                kq.FloorNumber = model.FloorNumber;
                kq.ServiceTypeId = model.ServiceTypeId;
                kq.Price = decimal.Parse(model.Price);
                kq.Description = model.Description;
                if (model.IsActive == 1)
                    kq.IsActive = true;
                else
                    kq.IsActive = false;
                kq.Deleted = kq.Deleted;
                kq.IsRoom = true;
                int gt = _service.UpdateService(kq);
                if (gt == 1)
                {
                    User team1 = KTQUyen();
                    string tile = team1.UserName + " đã cập nhật phòng " + model.Name;
                    _service.WriteLogAction(tile, team1.Id);
                    return RedirectToAction("ListServiceRoom", "Admin");
                }
                else
                    return View();
            }
            catch (Exception e) 
            { 
                return View();
            }
        }


        public ActionResult AddNewSevicetypeRoom()
        {
            User team1 = KTQUyen();
            if (team1 == null)
                return RedirectToAction("Login", "Login");
            else
            {
                if (team1.PermissonId == 2)
                    return RedirectToAction("QuyenTruyCap", "Admin");
                else
                {
                    return View();
                }
            }
        }

        [HttpPost]
        public ActionResult AddNewSevicetypeRoom(ServiceTypeModel model)
        {
            try
            {
                ServiceType team = new ServiceType();
                team.Name = model.Name;
                int kq = _service.InsertSeviceType(team);
                if (kq == 1)
                {
                    User team1 = KTQUyen();
                    string tile = team1.UserName + " đã thêm loại phòng " + model.Name;
                    _service.WriteLogAction(tile, team1.Id);
                    return RedirectToAction("Index", "Admin");
                }
                else
                    return View();
            }
            catch (Exception e) {
                return View();
            }

        }

        #endregion

        //Bao cao
        #region Xem Bao cao
        public ActionResult ReportAddmin()
        {
            User team1 = KTQUyen();
            if (team1 == null)
                return RedirectToAction("Login", "Login");
            else
            {
                if (team1.PermissonId == 2)
                    return RedirectToAction("QuyenTruyCap", "Admin");
                else
                {
                    ListReportModel model = new ListReportModel();
                    List<ReportEntity> List = _service.GetReportNowDay(null, null);
                    decimal SumPriceReport = 0;
                    foreach (ReportEntity gt in List)
                    {
                        ReportModel team = new ReportModel();
                        team.Rom = gt.Rom;
                        team.Name = gt.Name;
                        team.CMND = gt.CMND;
                        team.StarDay = gt.StarDay;
                        team.EndDay = gt.EndDay;
                        team.PriceRom = gt.PriceRom;
                        team.NameService = gt.NameService;
                        team.PriceService = gt.PriceService;
                        team.SumPrice = gt.SumPrice;
                        model.ReportDetail.Add(team);
                        SumPriceReport = SumPriceReport + Convert.ToDecimal(gt.SumPrice);
                    }
                    model.SumPriceReport = SumPriceReport.ToString();
                    model.StarDaySearch = "";
                    model.EndDaySearch = "";
                    return View(model);
                }
            }
        }
        
        [HttpPost]
        public ActionResult ReportAddmin(ListReportModel model)
        {
            if (model.StarDaySearch == "")
                model.StarDaySearch = null;
            if(model.EndDaySearch=="")
                model.EndDaySearch=null;
            List<ReportEntity> List = _service.GetReportNowDay(model.StarDaySearch, model.EndDaySearch);
            decimal SumPriceReport = 0;
            foreach (ReportEntity gt in List)
            {
                ReportModel team = new ReportModel();
                team.Rom = gt.Rom;
                team.Name = gt.Name;
                team.CMND = gt.CMND;
                team.StarDay = gt.StarDay;
                team.EndDay = gt.EndDay;
                team.PriceRom = gt.PriceRom;
                team.NameService = gt.NameService;
                team.PriceService = gt.PriceService;
                team.SumPrice = gt.SumPrice;
                model.ReportDetail.Add(team);
                SumPriceReport = SumPriceReport + Convert.ToDecimal(gt.SumPrice);
            }
            model.SumPriceReport = SumPriceReport.ToString();
            return View(model);
        }
        #endregion

    }
}
