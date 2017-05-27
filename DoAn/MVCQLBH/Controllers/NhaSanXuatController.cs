using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCQLBH.Models;

namespace MVCQLBH.Controllers
{
    public class NhaSanXuatController : Controller
    {
        // GET: NhaSanXuat
        public ActionResult GetListNSX()
        {
            using (var dc = new QLBHEntities())
            {
                var l = dc.NhaSanXuats.ToList();

                // Vì ta đặt tên cho PartialView của Action GetListNSX là ListByNSX, khác với 
                // tên của Action nên ta phải truyền thêm tham số là tên của PartialView tự đặt,
                // nên sẽ là return PartialView("TênPartialViewTựĐặt", list);
                return PartialView("_PartialListNSX", l);
            }
        }

    }
}