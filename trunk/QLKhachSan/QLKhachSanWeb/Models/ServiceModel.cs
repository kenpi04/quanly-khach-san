using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLKhachSanWeb.Models
{
    public class ServiceModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tầng")]
        public string FloorNumber { get; set; }

        public bool IsRoom { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập loại phòng")]
        public int ServiceTypeId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá")]
        public string Price { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mô tả")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập loại hoạt động")]
        public int IsActive { get; set; }
        public int Deleted { get; set; }
        public string ServiceType { get; set; }

    }
}