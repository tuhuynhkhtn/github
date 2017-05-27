using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCQLBH.Models
{
    public class Cart
    {
        public IList<CartItem> Items { get; }
        public Cart()
        {
            Items = new List<CartItem>();
        }

        public int TotalQuantity()
        {
            return Items.Sum(i => i.Quantity);
        }

        public void AddItem(int proId, int quantity = 1)
        {
            using (var dc = new QLBHEntities())
            {
                var pro = dc.Products.Where(p => p.ProID == proId).FirstOrDefault();
                if(pro != null)
                {
                    var ci = Items.Where(i => i.Product.ProID == proId).FirstOrDefault();
                    if(ci == null)
                    {
                        ci = new CartItem { Product = pro, Quantity = quantity };
                        Items.Add(ci);
                    }          
                    else
                    {
                        ci.Quantity += quantity;
                    }     
                }
            }
        }

        public void Remove(int pId)
        {
            var ci = Items.Where(i => i.Product.ProID == pId).FirstOrDefault();
            if(ci != null)
            {
                Items.Remove(ci);
            }
        }

        public void Update(int pId, int q)
        {
            var ci = Items.Where(i => i.Product.ProID == pId).FirstOrDefault();
            if (ci != null)
            {
                ci.Quantity = q;
            }
        }

        public void Checkout()
        {
            Items.Clear();
        }
    }

    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}