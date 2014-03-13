using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLKhachSanWeb.Models
{
    public class ServiceTypeModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên loại phòng")]
        public string Name { get; set; }
    }
}