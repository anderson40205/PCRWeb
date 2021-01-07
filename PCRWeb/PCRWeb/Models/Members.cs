using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PCRWeb.Models
{
    public class Members
    {
        [DisplayName("帳號")]
        [Required(ErrorMessage = "請輸入帳號")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "帳號長度須介於6-30字元")]
        [Remote("AccountCheck", "Members", ErrorMessage = "此帳號已被註冊過")]
        public string Account { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
    }
}