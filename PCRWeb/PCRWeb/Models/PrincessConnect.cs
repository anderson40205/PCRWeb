using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PCRWeb.Models
{
    public class PrincessConnect
    {
        [DisplayName("編號:")]
        public int Id { get; set; }
        public string d1 { get; set; }
        public string d2 { get; set; }
        public string d3 { get; set; }
        public string d4 { get; set; }
        public string d5 { get; set; }
        public string a1 { get; set; }
        public string a2 { get; set; }
        public string a3 { get; set; }
        public string a4 { get; set; }
        public string a5 { get; set; }
        public DateTime? insertTime { get; set; }//? 代表可以null
        public string Name { get; set; }

        [DisplayName("留言內容:")]
        [Required(ErrorMessage = "請輸入留言內容")]
        [StringLength(100, ErrorMessage = "留言內容不可超過100字元")]
        public string Content{ get; set; }
        public int positive { get; set; }
        public int negative { get; set; }
        
    }
}