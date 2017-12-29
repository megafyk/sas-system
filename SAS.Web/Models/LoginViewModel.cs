using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAS.Web.Models
{
    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}