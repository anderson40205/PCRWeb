using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCRWeb.Models
{
    public class ButtonModel
    {
        public bool State { get; set; }
        public ButtonModel(bool state)
        {
            State = state;
        }
    }
}