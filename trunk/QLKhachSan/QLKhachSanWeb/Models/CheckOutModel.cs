﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLKhachSanWeb.Models
{
    public class CheckOutModel
    {
        public CheckOutModel()
        {
            Rooms = new List<SelectListItem>();
            Services = new List<BookingInfoDetailModel>();
            AddServiceModel = new BookingInfoDetailModel();
            ServicesList = new List<SelectListItem>();
            BookingInfo = new BookingModel.BookingInfoModel();

        }
        public RoomModel Room { get; set; }
        public BookingModel.BookingInfoModel BookingInfo { get; set; }
        public IList<SelectListItem> Rooms { get; set; }
        
        public IList<BookingInfoDetailModel> Services { get; set; }
        public IList<SelectListItem> ServicesList { get; set; }
        public BookingInfoDetailModel AddServiceModel { get; set; }
        public class BookingInfoDetailModel 
        {
            public BookingInfoDetailModel()
            {
                Quatity = 1;
            }
            public int Id { get; set; }
           
            public int BookingInfoId { get; set; }
            [Display(Name="Dịch vụ")]
            [Required(ErrorMessage="Chọn dịch vụ")]
            public int ServiceId { get; set; }        
            [Display(Name="Giá")]
            [DisplayFormat(DataFormatString="{0:0,0}",ApplyFormatInEditMode=false)]
            public decimal Price { get; set; }
            [Display(Name="Số lượng")]
            [Required(ErrorMessage = "Nhập số lượng")]
            public decimal Quatity { get; set; }
            [Display(Name="Tổng cộng")]
            [Required(ErrorMessage = "Nhập tổng tiền")]
            [DisplayFormat(DataFormatString = "{0:0,0}", ApplyFormatInEditMode = false)]
            public decimal Total { get; set; }
            [Display(Name="Ghi chú")]
            public string Note { get; set; }
            public string ServiceName { get; set; }

            public object CreateOnDate { get; set; }
        }

        public decimal Total { get; set; }
    }
}