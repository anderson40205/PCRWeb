using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using PCRWeb.Models;

namespace PCRWeb.ViewModels
{
    public class PrincessConnectViewModel
    {
        public bool State { get; set; }
        [DisplayName("搜尋:")]
        public string Search { get; set; }
        public string content { get; set; }
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
        public List<PrincessConnect> DataList { get; set; }
    }
}