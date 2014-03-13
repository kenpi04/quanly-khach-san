using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLKhachSanWeb.Models
{
    public class LogModel
    {
        public int id { get; set; }
        public string ActionName { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public string CreateDate { get; set; }
    }
}