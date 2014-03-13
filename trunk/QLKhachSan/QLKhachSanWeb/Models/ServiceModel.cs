using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLKhachSanWeb.Models
{
    public class ServiceModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FloorNumber { get; set; }
        public bool IsRoom { get; set; }
        public int ServiceTypeId { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }
        public int IsActive { get; set; }
        public int Deleted { get; set; }
        public string ServiceType { get; set; }

    }
}