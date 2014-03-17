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
           
            if (_service.CheckRoomContent(model.BookingInfo.RoomId, model.BookingInfo.CheckingDate) || _service.CheckRoomContent(model.BookingInfo.RoomId, model.BookingInfo.CheckOutDate))
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
               UserId = ((User)Session["SessionUser"]).Id
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
            var room = _service.GetRoomById(entity.RoomId);
            var bookingDetail = new BookingInfoDetail
            {
                BookingInfoId=entity.Id,
                ServiceId=room.Id,
                Note="Tiền phòng",
                Price=room.Price,
                Quatity=1,
                Total=room.Price,
                ServiceName=room.Name
               
            };
            _service.InsertBookingInfoDetail(bookingDetail);
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
        public ActionResult GetListBookingInfo(int roomId,bool dango=false)
        {
            var model=new List<SelectListItem>();
            if(!dango)
             model = _service.GetListBookingInfoByRoomId(roomId).Where(y=>y.StatusId==(int)BookingInfoStatusEnums.DamBao||y.StatusId==(int)BookingInfoStatusEnums.KoDamBao).Select(x => new SelectListItem { Text = x.CustomerName, Value = x.Id.ToString() }).ToList();
            else
                model = _service.GetListBookingInfoByRoomId(roomId).Where(y => y.StatusId == (int)BookingInfoStatusEnums.PhongDangO).Select(x => new SelectListItem { Text = x.CustomerName, Value = x.Id.ToString() }).ToList();
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
            int numdate = (int)(DateTime.Now - booking.CheckingDate.Value).TotalDays;
            //var bookingDetailRoom = _service.GetBookingInfoDetailByBookingInfoId(bookingId).FirstOrDefault(x => x.ServiceId == room.Id);
            //if (bookingDetailRoom == null)
            //    return Content("");
            //bookingDetailRoom.Quatity = numdate;
            //bookingDetailRoom.Total = bookingDetailRoom.Price * numdate;
            _service.UpdateService(room);
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
         
            model.Total =model.Services.Sum(x => x.Total);
            model.ServicesList = _service.GetServiceS().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList();
            model.ServicesList.Insert(0, new SelectListItem { Text = room.Name, Value = room.Id.ToString() });
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
            int i = 0; string a = "Thêm";
            if (model.AddServiceModel.Id > 0)
            {
                entity.Id = model.AddServiceModel.Id;
                i = _service.UpdateBookingInfoDetail(entity);
                a = "Cập nhập";
            }
            else
                 i=  _service.InsertBookingInfoDetail(entity);
          if (i == 1)
          {
           var bookingInfo=_service.GetBookingInforById(entity.BookingInfoId);
              var room=_service.GetRoomById(bookingInfo.RoomId);
              string action = string.Format(a+" {0} dịch vụ {1} cho phòng {2} với giá {3}", entity.Quatity, service.Name, room.Name, entity.Price);
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
                CustomerName=bookingInfo.CustomerName
                
            };        
            model.Total =model.Services.Sum(x => x.Total);
            string action = string.Format("Thanh toán cho phòng{0} số tiền {1}",room.Name,model.Total);
            _service.WriteLogAction(action, ((User)Session["SessionUser"]).Id);
            return PartialView("_CheckOutDetail", model);

           


        }
        [HttpGet]
        public ActionResult EditService(CheckOutModel model)
        {
            var entity = _service.GetBookingInfoDetailById(model.AddServiceModel.Id);
            if (entity == null)
                return Json("Không tìm thấy dịch vụ!");
            entity.Price = model.AddServiceModel.Price;
            entity.Quatity = model.AddServiceModel.Quatity;
            entity.Total = model.AddServiceModel.Total;
            int i = _service.UpdateBookingInfoDetail(entity);
            if (i != 0)
                return Json("Cập nhật thành công");
            return Json("Cập nhật không thành công");
        }
        [HttpPost]
        public ActionResult DeleteService(int id) {
            var entity = _service.GetBookingInfoDetailById(id);
            if (entity == null)
                return Json("Không tìm thấy dịch vụ");
            if (_service.DeleteBookingInfoDetail(entity) == 1)
                return Json("Xóa thành công");
            return Json("Xóa không thành công");
                

        }
        [HttpPost]
        public ActionResult ChangeRoom(int bookingId,int roomChangeId)
        {
            var booking = _service.GetBookingInforById(bookingId);            
            if (booking == null)
                return Json("Thông tin không tồn tại!");
            if (_service.CheckRoomContent(roomChangeId, booking.CheckingDate.Value) || _service.CheckRoomContent(roomChangeId, booking.CheckOutDate.Value))
                return Json("Phòng đã có người đặt");
            booking.HasChangeRoom=true;
            booking.StatusId = (int)BookingInfoStatusEnums.PhongDaCheckOut;
          int i=  _service.UpdateBookingInfo(booking);
           if(i==0)
               return Json("Cập nhật thông tin booking không thành công!");
           var roomChan = _service.GetRoomById(roomChangeId);
            var newBooking = new BookingInfo { 
                FromBookingInfoId=booking.Id,
                BookingDate=DateTime.Now,
                CheckingDate=DateTime.Now,
                CheckOutDate=booking.CheckOutDate,
                CustomerName=booking.CustomerName,
                CustomerCardNo=booking.CustomerCardNo,
                CustomerInfoOther=booking.CustomerInfoOther,
                PhoneNumber=booking.PhoneNumber,
                UserId=((User)Session["SessionUser"]).Id,
                RoomId = roomChangeId,
                LastUpdateDate=DateTime.Now,
               
            };
        i=  _service.InsertBookingInfo(newBooking);
          if (i == 0)
              return Json("Thêm mới không thành công!");
          var newBookAfterInser = _service.GetListBookingInfoByRoomId(roomChangeId).FirstOrDefault(x => x.FromBookingInfoId == booking.Id);
          var bookingDetailNew = new BookingInfoDetail
          {
              BookingInfoId=newBookAfterInser.Id,
              Price=roomChan.Price,
              Quatity=1,
              ServiceId=roomChan.Id,
              ServiceName=roomChan.Name,
              Total=roomChan.Price
              
          };
          i = _service.InsertBookingInfoDetail(bookingDetailNew);
          if (i == 0)          
              return Json("Thêm thông tin mới ko thành công");
          string action = string.Format("Chuyển khách {0} từ {1} đến {2}",booking.CustomerName,_service.GetRoomById(booking.RoomId).Name,roomChan.Name);
          _service.WriteLogAction(action, ((User)Session["SessionUser"]).Id);
            return Json(new
            {
               status=1,
               mes = "Chuyển đến " + roomChan.Name + " thành công!"
            });


        }
       
        

    }
}
