using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCQLBH.Models
{
    public partial class Product
    {
        [AllowHtml]
        public string FullDesRaw { get; set; }
    }
}