﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLKhachSanWeb.Models
{
    public class ReportModel
    {
       
        public string Rom { get; set; }
        public string Name { get; set; }
        public string CMND { get; set; }
        public string StarDay{get;set;}
        public string EndDay { get; set; }
        public string PriceRom{get;set;}
        public string NameService{get;set;}
        public string PriceService{get;set;}
        public string SumPrice { get; set; }

    }
}