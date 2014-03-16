using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityLibrary;
using QLKhachSanWeb.Models;
using QLKhachSanWeb.Helper;

namespace QLKhachSanWeb.Controllers
{
    [Auth]
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        ThienService service = new ThienService();
        int KTQuyenUser()
        {
            int kq = 0;
            if (this.Session["SessionUser"] != null)
            {
                User nd = (User)this.Session["SessionUser"];
                kq = nd.PermissonId;
            }
            return kq;
        }
        public ActionResult Index()
        {
           // ViewBag.QuyenUser = KTQuyenUser();
            ViewBag.QuyenUser = 1;
            return View();
        }

        public ActionResult ShowBookingInfo(DateTime? startDate,DateTime? endDate)
        {
            var model = new ListBookingModel();
            if (!startDate.HasValue)
                startDate = DateTime.Now.AddDays(-4);
            if (!endDate.HasValue)
                endDate = DateTime.Now.AddDays(10);
            model.DateStart = startDate.Value;
            model.DateEnd = endDate.Value;
            model.Rooms = service.GetRooms().Select(x =>
            {
                return new RoomModel
                {
                    Id=x.Id,
                    TypeId=x.ServiceTypeId,
                    Name=x.Name,
                    Price=x.Price,
                    BookingInfo=GetBookingInfoModels(x.Id)
                };
            }).ToList();
            return PartialView("ListBookingInfo", model);
           
        }
        private List<RoomModel.BookingInfoModel> GetBookingInfoModels(int roomId)
        {
            var model = new List<RoomModel.BookingInfoModel>();
            model = service.GetListBookingInfoByRoomId(roomId).Select(x =>
            {
                return new RoomModel.BookingInfoModel
                {
                    Id=x.Id,
                    StartDate=x.CheckingDate.Value,
                    EndDate=x.CheckOutDate.Value,
                    TypeId=x.StatusId
                };
            }).ToList();
            return model;
            
        }
        public ActionResult DangXuat()
        {
            HttpContext.Session["SessionUser"] = null;
            return RedirectToAction("Login", "Login");
        }


        ThienGetService123 _service = new ThienGetService123();
        public ActionResult ReportUser()
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
        [HttpPost]
        public ActionResult ReportUser(ListReportModel model)
        {
            if (model.StarDaySearch == "")
                model.StarDaySearch = null;
            if (model.EndDaySearch == "")
                model.EndDaySearch = null;
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
       

    }
}
