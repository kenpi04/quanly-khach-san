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
               UserId=((User)Session["User"]).Id
            };
            if (_service.InsertBookingInfo(entity) == 1)
                return Json("success");
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
            return 1;
        }
       
        

    }
}
