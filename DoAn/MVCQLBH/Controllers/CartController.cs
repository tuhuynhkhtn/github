using MVCQLBH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCQLBH.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult DetailCart()
        {
            var cart = Session["cart"] as Cart;
            return View(cart);
        }

        // POST: Cart/AddCIFomDetail
        [HttpPost]
        public ActionResult AddCIFomDetail(int proId, int quantity)
        {
            if (Session["cart"] == null)
            {
                Session["cart"] = new Cart();
            }
            var c = Session["cart"] as Cart;
            c.AddItem(proId, quantity);

            //using (var dc = new QLBHEntities())
            //{
            //    var pro = dc.Products.Where(p => p.ProID == proId).FirstOrDefault();
            //    if(pro != null)
            //    {
            //        c.Items.Add(new CartItem { Product = pro, Quantity = quantity });
            //    }
            //}

            return RedirectToAction("DetailPro", "Product", new { id = proId });
        }

        // POST: Cart/AddCIFromListProduct
        [HttpPost]
        public ActionResult AddCIFromListProduct(int proId, int catId, int page)
        {
            if (Session["cart"] == null)
            {
                Session["cart"] = new Cart();
            }
            var c = Session["cart"] as Cart;
            c.AddItem(proId);
            return RedirectToAction("GetListByCategory", "Product", new { id = catId, page = page });
        }

        // POST: Cart/AddNSXIFromListProduct
        [HttpPost]
        public ActionResult AddNSXIFromListProduct(int proId, int nsxId, int page)
        {
            if (Session["cart"] == null)
            {
                Session["cart"] = new Cart();
            }
            var c = Session["cart"] as Cart;
            c.AddItem(proId);
            return RedirectToAction("GetListByNSX", "Product", new { id = nsxId, page = page });
        }

        // POST: Cart/Remove
        [HttpPost]
        public ActionResult Remove(int proId)
        {
            var c = Session["cart"] as Cart;
            c.Remove(proId);
            return RedirectToAction("DetailCart", "cart");
        }

        // POST: Cart/Update
        [HttpPost]
        public ActionResult Update(int proId, int quantity)
        {
            var c = Session["cart"] as Cart;
            c.Update(proId, quantity);
            return RedirectToAction("DetailCart", "cart");
        }

        // POST: Cart/Update
        [HttpPost]
        public ActionResult Checkout()
        {
            var c = Session["cart"] as Cart;
            var ui = Session["Logged"] as UserInfo;

            using (var dc = new QLBHEntities())
            {
                var user = dc.Users.Where(u => u.f_Username == ui.Username).FirstOrDefault();
                var order = new Order
                {
                    OrderDate = DateTime.Now,
                    User = user,
                };
                dc.Orders.Add(order);

                decimal amount = 0, total = 0;

                foreach(var ci in c.Items)
                {
                    var p = dc.Products.Where(i => i.ProID == ci.Product.ProID).FirstOrDefault();

                    // Cập nhật số lượng bán, số lượng tồn
                    p.Quantity -= 1;
                    p.SoLuongDaBan += 1;
                    dc.SaveChanges();

                    amount = p.Price * ci.Quantity;
                    total += amount;

                    var od = new OrderDetail
                    {
                        Order = order,
                        Product = p,
                        Quantity = ci.Quantity,
                        Price = p.Price,
                        Amount = amount
                    };
                    dc.OrderDetails.Add(od);
                }
                order.Total = total;
                dc.SaveChanges();
            }

                c.Checkout();
            return RedirectToAction("DetailCart", "cart");
        }

    }
}