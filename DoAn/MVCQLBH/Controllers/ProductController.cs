using MVCQLBH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCQLBH.Controllers
{
    public class ProductController : Controller
    {
        static int nPerPage = 6; // Số lượng sản phẩm tối đa hiển thị trên 1 trang
        // GET: Product
        public ActionResult GetListByCategory(int? id, int page = 1)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index", "Home");
            }
            using (var dc = new QLBHEntities())
            {
                // totalP là tổng số Product
                int totalP = dc.Products.Where(p => p.CatID == id).Count();

                if(totalP == 0)
                {
                    return View("ListByCategory", new List<Product>());
                }

                // Tính tổng số trang phải hiển thị
                int nPage = totalP / nPerPage + (totalP % nPerPage > 0 ? 1 : 0);

                if (page < 1)
                {
                    page = 1;
                }
                if (page > nPage)
                {
                    page = nPage;
                }

                ViewBag.totalPage = nPage;
                ViewBag.curPage = page;
                ViewBag.cId = id;

                var l = dc.Products
                    .Where(p => p.CatID == id)
                    .OrderBy(p => p.ProID)
                    .Skip((page - 1) * nPerPage)
                    .Take(nPerPage)
                    .ToList();
                return View("ListByCategory", l);
            }
        }

        public ActionResult GetListByNSX(int? id, int page = 1)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index", "Home");
            }
            using (var dc = new QLBHEntities())
            {
                // totalP là tổng số Product
                int totalP = dc.Products.Where(p => p.IDNhaSanXuat == id).Count();

                // Tính tổng số trang phải hiển thị
                int nPage = totalP / nPerPage + (totalP % nPerPage > 0 ? 1 : 0);

                if (page < 1)
                {
                    page = 1;
                }
                if (page > nPage)
                {
                    page = nPage;
                }

                ViewBag.totalPage = nPage;
                ViewBag.curPage = page;
                ViewBag.nsxId = id;

                var l = dc.Products
                    .Where(p => p.IDNhaSanXuat == id)
                    .OrderBy(p => p.ProID)
                    .Skip((page - 1) * nPerPage)
                    .Take(nPerPage)
                    .ToList();
                return View("ListByNSX", l);
            }               
        }

        public ActionResult DetailPro(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index", "Home");
            }

            using (var dc = new QLBHEntities())
            {
                var product = dc.Products.Where(p => p.ProID == id).FirstOrDefault();

                product.SoLuotXem += 1;
                Session["IDPro"] = product.ProID;

                //var product = from p in dc.Products
                //              from n in dc.NhaSanXuats
                //              where p.IDNhaSanXuat == n.IDNhaSanXuat
                //              select new
                //              {
                //                  GiaBan = p.Price,
                //                  SoLuotXem = p.SoLuotXem,
                //                  SoLuongBan = p.Quantity,
                //                  MoTa = p.FullDes,
                //                  XuatXu = p.XuatXu,
                //                  Loai = p.Loai,
                //                  TenNhaSanXuat = n.TenNhaSanXuat
                //              };

                string ten = (from p in dc.Products
                              from n in dc.NhaSanXuats
                              where p.IDNhaSanXuat == n.IDNhaSanXuat && p.ProID == id
                              select n.TenNhaSanXuat).FirstOrDefault().ToString();
                ViewBag.TenNSX = ten;

                return View(product);
            }
        }
    }
}