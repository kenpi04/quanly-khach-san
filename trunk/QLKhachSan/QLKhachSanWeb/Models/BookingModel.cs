﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace QLKhachSanWeb.Models
{
    public class BookingModel
    {
        public BookingModel()
        {
            Room = new List<RoomModel>();
            BookingInfo=new BookingInfoModel();
        }
      public  IList<RoomModel> Room { get; set; }
      public BookingInfoModel BookingInfo { get; set; }

      public class BookingInfoModel {
          public int Id { get; set; }
          [Display(Name = "Tên khách hàng")]
          [Required(ErrorMessage = "vui lòng nhập tên")]
          [MaxLength(50,ErrorMessage="Tên tối đa 50 ký tự")]
          public string CustomerName { get; set; }

          [Display(Name = "Số CMND")]
          [MaxLength(20, ErrorMessage = "Số CMND tối đa 20 ký tự")]
          public string CustomerCardNo { get; set; }


          [Display(Name = "Số điện thoại")]
          [MaxLength(20, ErrorMessage = "Số điện thoại tối đa 20 ký tự")]
          public string PhoneNumber { get; set; }
          [Display(Name = "Thông tin khác")]
          [MaxLength(200, ErrorMessage = "Ghi chú tối đa 200 ký tự")]
          public string CustomerInfoOther { get; set; }
          
        
          public DateTime BookingDate { get; set; }

          [Display(Name = "Ngày nhận phòng")]
          [Required(ErrorMessage = "Nhập ngày nhận phòng")]
         
          public string CheckingDate { get; set; }

          [Display(Name = "Ngày trả phòng")]
          [Required(ErrorMessage = "Nhập ngày trả phòng")]      
          public string CheckOutDate { get; set; }
          [Display(Name = "Phòng")]
          public int RoomId { get; set; }

          public short StatusId { get; set; }
      }
  

    }
}