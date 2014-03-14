using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLKhachSanWeb.Models
{
    public class CheckInModel
    {

        public IList<SelectListItem> Rooms { get; set; }
        public IList<SelectListItem> Status { get; set; }
     
      
    }
}