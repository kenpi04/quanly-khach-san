using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLKhachSanWeb.Models
{
    public class RoomModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TypeId { get; set; }
        public decimal Price { get; set; }
        public string TypeName { get; set; }
        public string FloorNumber { get; set; }
        public IList<BookingInfoModel> BookingInfo { get; set; }
        public class BookingInfoModel
        {
            public int Id { get; set; }
            public int TypeId { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string CustomerName { get; set; }
        }
    }
}