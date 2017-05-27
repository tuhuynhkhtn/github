using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCQLBH.Models;

namespace MVCQLBH.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult GetList()
        {
            using (var dc = new QLBHEntities())
            {
                var l = dc.Categories.ToList();
                return PartialView("_PartialList", l);
            }
        }
    }
}