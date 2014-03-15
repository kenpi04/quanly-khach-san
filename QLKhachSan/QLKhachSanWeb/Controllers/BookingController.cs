using EntityLibrary;
using QLKhachSanWeb.Helper;
using QLKhachSanWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLKhachSanWeb.Controllers
{
    [Auth]
    public class BookingController : Controller
    {

        ThienService _service = new ThienService();
        public ActionResult BookRoom()
        {
            var model = new BookingModel();
            model.Room = _service.GetRooms().Select(x =>
            {
                return new RoomModel
                {
                    Id = x.Id,
                    TypeId = x.ServiceTypeId,
                    Name = x.Name,
                    Price = x.Price,
                 
                };
            }).ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult BookRoom(BookingModel model)
        {
           
            if (_service.CheckRoomContent(model.BookingInfo.RoomId, model.BookingInfo.CheckingDate) || _service.CheckRoomContent(model.BookingInfo.RoomId, model.BookingInfo.CheckingDate))
                return Json("Phòng đã có người đặt");
            var entity = new BookingInfo
            {
               CustomerName=model.BookingInfo.CustomerName,
               CustomerCardNo=model.BookingInfo.CustomerCardNo,
               PhoneNumber=model.BookingInfo.PhoneNumber,
               RoomId=model.BookingInfo.RoomId,
               CheckingDate=model.BookingInfo.CheckingDate,
               CheckOutDate=model.BookingInfo.CheckOutDate,
               StatusId=model.BookingInfo.StatusId,
               BookingDate=DateTime.Now,
               CreatedDate=DateTime.Now,
               LastUpdateDate=DateTime.Now,
               UserId=((User)Session["SessionUser"]).Id
            };
            if (_service.InsertBookingInfo(entity) == 1)
            {
                _service.WriteLogAction("Book phòng", entity.UserId);
                return Json("success");
            }
            return Json("fail");
                   
               
          

        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            var model = _service.GetBookingInforById(id);
            if (model == null)
                return Content("");
            return PartialView("_BookingDetail", model);
        }
        [HttpGet]
        public ActionResult RoomDetail(int id)
        {
            var entity = _service.GetRoomById(id);
            if (entity == null)
                return Json("");
            var model = new RoomModel
            {
                Id=entity.Id,
                Name=entity.Name,
                TypeName=_service.GetServiceTypeById(entity.ServiceTypeId).Name,
                FloorNumber=entity.FloorNumber
                

            };
                       
            return PartialView("_RoomDetail", model);

        }
        [HttpPost]
        public int ChangeStatus(int id, int status)
        {
            var entity = _service.GetBookingInforById(id);
            if (entity == null)
                return 0;
            entity.StatusId =(short)status;
            _service.UpdateBookingInfo(entity);
            string action=string.Format("Khách {0} đến nhận phòng {1}",entity.CustomerName,_service.GetRoomById(entity.RoomId).Name);
            _service.WriteLogAction(action, ((User)Session["SessionUser"]).Id);
            return 1;
        }

        public ActionResult CheckIn()
        {
            var model = new CheckInModel();
            model.Rooms = _service.GetRooms().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
            return View(model);
        }
        [HttpGet]
        public ActionResult GetListBookingInfo(int roomId)
        {
            var model = _service.GetListBookingInfoByRoomId(roomId).Where(y=>y.StatusId==(int)BookingInfoStatusEnums.DamBao||y.StatusId==(int)BookingInfoStatusEnums.KoDamBao).Select(x => new SelectListItem { Text = x.CustomerName, Value = x.Id.ToString() }).ToList();
            return Json(model,JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult GetListBookingInfoDetail(int bookingId)
        {
            var booking = _service.GetBookingInforById(bookingId);
            if (booking == null)
                return Json("Thông tin không hợp lệ!");
            var model = new CheckOutModel();
            var room = _service.GetRoomById(booking.RoomId);
            model.Room = new RoomModel
            {
                Id=room.Id,
                Name=room.Name,
                TypeName=_service.GetServiceTypeById(room.ServiceTypeId).Name,
                Price=room.Price
            };
            model.Services = _service.GetBookingInfoDetailByBookingInfoId(bookingId).Select(x => new CheckOutModel.BookingInfoDetailModel
            {
                Id=x.Id,
                ServiceId=x.ServiceId,
                Quatity=x.Quatity,
               Price=x.Price,
               Note=x.Note,
               Total=x.Total,
               ServiceName=x.ServiceName
             
            }).ToList();
            model.Total = model.Room.Price + model.Services.Sum(x => x.Total);
            model.ServicesList = _service.GetServiceS().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList();
            return PartialView("_ListServices", model);
        }
        [HttpPost]
        public ActionResult InsertService(CheckOutModel model)
        {
            var service = _service.ServicebyId(model.AddServiceModel.ServiceId);
            if (service == null)
                return Json("Dịch vụ không tồn tại");
            var entity = new BookingInfoDetail
            {
                BookingInfoId = model.AddServiceModel.BookingInfoId,
                Note = model.AddServiceModel.Note,
                Price = service.Price,
                Quatity = model.AddServiceModel.Quatity,
                Total = model.AddServiceModel.Total,
                ServiceName=service.Name,
                ServiceId=model.AddServiceModel.ServiceId
            };
          int i=  _service.InsertBookingInfoDetail(entity);
          if (i == 1)
          {
           var bookingInfo=_service.GetBookingInforById(entity.BookingInfoId);
              var room=_service.GetRoomById(bookingInfo.RoomId);
              string action = string.Format("Thêm {0} dịch vụ {1} cho phòng {2} với giá {3}", entity.Quatity, service.Name, room.Name, entity.Price);
              _service.WriteLogAction(action, ((User)Session["SessionUser"]).Id);
              return Json("success");
          }
        return Json("Lỗi! không thành công");
            
        }
        [HttpGet]
        public decimal GetPrice(int serviceId)
        {
            return _service.ServicebyId(serviceId).Price;
        }
        public ActionResult CheckOut()
        {
            var model = new CheckOutModel();
            model.Rooms = _service.GetRooms().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
           
            return View(model);
        }
        [HttpPost]
        public ActionResult CheckOut(int bookingInfoId)
        {
            var bookingInfo = _service.GetBookingInforById(bookingInfoId);
            if (bookingInfo == null)
                return Json("Thông tin không tồn tại!");
            bookingInfo.StatusId = (int)BookingInfoStatusEnums.PhongDaCheckOut;
            int i = _service.UpdateBookingInfo(bookingInfo);
            if (i == 0)
                return Json("Lỗi không thành công!");

            var model = new CheckOutModel();
            model.Services = _service.GetBookingInfoDetailByBookingInfoId(bookingInfoId).Select(x => new CheckOutModel.BookingInfoDetailModel
            {
                Id = x.Id,
                ServiceId = x.ServiceId,
                Quatity = x.Quatity,
                Price = x.Price,
                Note = x.Note,
                Total = x.Total,
                ServiceName = x.ServiceName,

            }).ToList();
            var room = _service.GetRoomById(bookingInfo.RoomId);
            model.Room = new RoomModel
            {
                Id = room.Id,
                Name = room.Name,
                TypeName = _service.GetServiceTypeById(room.ServiceTypeId).Name,
                Price = room.Price
            };
            model.BookingInfo = new BookingModel.BookingInfoModel{ 
                Id=bookingInfo.Id,
                RoomId=bookingInfo.RoomId,
                CheckingDate=bookingInfo.CheckingDate.Value,
                CheckOutDate=bookingInfo.CheckOutDate.Value,
                CustomerName=bookingInfo.CustomerName,
                
            };
            model.Total = model.Services.Sum(x => x.Total);
            string action = string.Format("Thanh toán cho phòng{0} số tiền {1}",room.Name,model.Total);
            _service.WriteLogAction(action, ((User)Session["SessionUser"]).Id);
            return PartialView("_CheckOutDetail", model);

           


        }
       
        

    }
}
