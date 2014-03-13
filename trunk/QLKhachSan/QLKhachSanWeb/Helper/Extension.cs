using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLKhachSanWeb.Models;

namespace QLKhachSanWeb.Helper
{
    public static class Extension
    {
        public static bool CheckRoomContent(this RoomModel model, DateTime Date)
        {
            return (model.BookingInfo.Where(x => x.StartDate.Day >= Date.Day && x.EndDate.Day <= Date.Day).Count() > 0);
        }
    }
}