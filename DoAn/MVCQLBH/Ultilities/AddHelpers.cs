using MVCQLBH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCQLBH.Ultilities
{
    public static class AddHelpers
    {
        public static object HtttpContext { get; private set; }

        public static MvcHtmlString LessString(this HtmlHelper html, string str, int maxLength)
        {
            if(str.Length < maxLength)
            {
                return new MvcHtmlString(str);
            }
            return new MvcHtmlString(
                string.Format("{0}...", str.Substring(0, maxLength - 3)));
        }

        public static string Price2String(this HtmlHelper html, decimal price)
        {
            return string.Format("{0:N0} đ", price);
        }

        // Kiểm tra có đăng nhập chưa
        public static bool IsLogged(this HtmlHelper html)
        {
            if(HttpContext.Current.Session["Logged"] == null)
            {
                if(HttpContext.Current.Request.Cookies["UserId"] != null)
                {
                    int id = int.Parse(HttpContext.Current.Request.Cookies["UserId"].Value);
                    using (var dc = new QLBHEntities())
                    {
                        var user = dc.Users.Where(u => u.f_ID == id).FirstOrDefault();
                        if(user != null)
                        {
                            HttpContext.Current.Session["Logged"] = new UserInfo { Username = user.f_Username, Permission = user.f_Permission };
                            return true;
                        }
                    }
                }
                return false;
            }
            return true;
        }

        // Kiểm tra có phải là Admin đăng nhập không
        public static bool IsLoggedAdmin(this HtmlHelper html)
        {
            if (HttpContext.Current.Request.Cookies["UserId"] != null)
            {
                int id = int.Parse(HttpContext.Current.Request.Cookies["UserId"].Value);
                using (var dc = new QLBHEntities())
                {
                    var user = dc.Users.Where(u => u.f_ID == id && u.f_Permission == 1).FirstOrDefault();
                    if (user != null)
                    {
                        HttpContext.Current.Session["Logged"] = new UserInfo { Username = user.f_Username };
                        return true;
                    }
                }
            }
            return false;
        }

        // Kiểm tra hàng hết chưa
        public static bool IsOutOfStock(this HtmlHelper html, int IDPro)
        {
            //int id = int.Parse(HttpContext.Current.Request.Cookies["IDPro"].Value);
            using (var dc = new QLBHEntities())
            {
                var product = dc.Products.Where(u => u.ProID == IDPro && u.Quantity == 0).FirstOrDefault();
                if (product != null)
                {
                    return true;
                }
            }
            return false;
        }

        public static string GetUsername(this HtmlHelper html)
        {
            var ui = HttpContext.Current.Session["Logged"] as UserInfo;
            if(ui != null)
            {
                return ui.Username;
            }
            return "";
        }

        public static UserInfo GetUserInfo(this HtmlHelper html)
        {
            var ui = HttpContext.Current.Session["Logged"] as UserInfo;
            return ui;
        }

        public static Cart GetCart(this HtmlHelper html)
        {
            if(HttpContext.Current.Session["cart"] == null)
            {
                HttpContext.Current.Session["cart"] = new Cart();
            }
            return (Cart)HttpContext.Current.Session["cart"];
        }

        public static IList<SelectListItem> GetSLICat(this HtmlHelper html)
        {
            var l = new List<SelectListItem>();
            using (var dc = new QLBHEntities())
            {
                foreach(var c in dc.Categories.ToList())
                {
                    l.Add(new SelectListItem
                    {
                        Value = c.CatID.ToString(),
                        Text = c.CatName
                    });
                }
            }
            return l;
        }

    }
}