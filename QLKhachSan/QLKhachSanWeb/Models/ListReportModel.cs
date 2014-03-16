using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLKhachSanWeb.Models
{
    public class ListReportModel
    {
        public ListReportModel()
        {
            ReportDetail = new List<ReportModel>();
        }
        public string StarDaySearch { get; set; }
        public string EndDaySearch { get; set; }
        public List<ReportModel> ReportDetail;
        public string SumPriceReport { get; set; }

    }
}