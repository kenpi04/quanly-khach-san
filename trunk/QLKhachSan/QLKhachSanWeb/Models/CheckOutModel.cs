using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLKhachSanWeb.Models
{
    public class CheckOutModel
    {
        public IList<SelectListItem> Rooms { get; set; }
        
        public IList<BookingInfoDetailModel> Services { get; set; }
        public class BookingInfoDetailModel 
        {
            public int Id { get; set; }
            public int BookingInfoId { get; set; }
            public int ServiceId { get; set; }        
            public decimal Price { get; set; }
            public decimal Quatity { get; set; }
            public decimal Total { get; set; }
            public string Note { get; set; }
        }
    }
}