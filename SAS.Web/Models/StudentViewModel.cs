using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAS.Web.Models
{
    public class StudentViewModel
    {
        public string CoporationCode { get; set; }
        public string StudentId { get; set; }
        public string Name { get; set; }
        public string Grade { get; set; }
        public string Class { get; set; }
        public string Status { get; set; }
        public string UserId { get; set; }
    }
}