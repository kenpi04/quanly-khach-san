using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLKhachSanWeb.Models
{
    public class HomeModel
    {
        public IList<RoomModel> Rooms { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }
}