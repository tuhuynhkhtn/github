using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCQLBH.Models;
using MVCQLBH.Ultilities;

namespace MVCQLBH.Controllers
{
    [AuthActionFilter(RequiredPermission = 1)]

    public class ManageProductController : Controller
    {
        // GET: ManageProduct
        public ActionResult IndexManageProduct()
        {
            using (var dc = new QLBHEntities())
            {
                var l = dc.Products.ToList();
                return View(l);
            }
        }

        // GET: ManageProduct/Delete
        public ActionResult Delete(int id)
        {
            using (var dc = new QLBHEntities())
            {
                var pD = dc.Products.Where(p => p.ProID == id).FirstOrDefault();
                if(pD != null)
                {
                    dc.Products.Remove(pD);
                    dc.SaveChanges();
                }
                Ulti.DeleteProductImgs(id, Server.MapPath("~"));
                return RedirectToAction("IndexManageProduct");
            }
        }

        // GET: ManageProduct/Add
        public ActionResult Add()
        {
            var p = new Product
            {
                ProName = "product",
                CatID = 3,
                Price = 100000,
                Quantity = 2,
                TinyDes = "tinydes..."

            };
            return View(p);
        }

        // POST: ManageProduct/Add
        [HttpPost]
        public ActionResult Add(Product p, HttpPostedFileBase imgLg, HttpPostedFileBase imgSm)
        {
            if(p.TinyDes == null)
            {
                p.TinyDes = string.Empty;
            }
            if (p.FullDesRaw == null)
            {
                p.FullDesRaw = string.Empty;
            }
            p.FullDes = p.FullDesRaw;
            if (p.NgayNhap == null)
            {
                p.NgayNhap = DateTime.Now.Date;
            }
            if (p.SoLuotXem == null)
            {
                p.SoLuotXem = 0;
            }
            using (var dc = new QLBHEntities())
            {
                dc.Products.Add(p);
                dc.SaveChanges();
            }
            Ulti.SaveProductImgs(p.ProID, Server.MapPath("~"), imgLg, imgSm);
            return RedirectToAction("IndexManageProduct");
        }

    }
}