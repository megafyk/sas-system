using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAS.Web.Models
{
    public class AnnouncementViewModel
    {
        public int AnnounceID { get; set; }
        public DateTime ScheduleDate { get; set; }
        public DateTime PostDate { get; set; }
        public string Subject { get; set; }
        public string Contents { get; set; }
        public string Rcv_Copo { get; set; }
        public string Rcv_Group { get; set; }
        public string Rcv_Class { get; set; }
        public string Rcv_User { get; set; }
        public string UserClass { get; set; }
        public string Status { get; set; }
    }
}