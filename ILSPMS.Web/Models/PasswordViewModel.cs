using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ILSPMS.Web.Models
{
    public class PasswordViewModel
    {
        public string Current { get; set; }
        public string New { get; set; }
        public string Confirm { get; set; }
    }
}