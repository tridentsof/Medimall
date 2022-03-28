using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Medimall.Models
{
    public class CartItem
    {
        public Product _shopping_product { get; set; }
        public int _shopping_quantity { get; set; }
    }

    //Gio hang
    public class Cart
    {
        List<CartItem> items = new List<CartItem>();

        public IEnumerable<CartItem> Items
        {
            get { return items; }
        }

        public void Add(Product _pro, int _quantity = 1)
        {
            var item = items.FirstOrDefault(p => p._shopping_product.ProductId == _pro.ProductId);

            if (item == null)
            {
                items.Add(new CartItem
                {
                    _shopping_product = _pro,
                    _shopping_quantity = _quantity
                });
            }

            else
            {
                item._shopping_quantity += _quantity;
            }
        }

        public void UpdateQuantity(int id, int _quantity)
        {
            var item = items.Find(p => p._shopping_product.ProductId == id);

            if (item != null)
            {
                item._shopping_quantity = _quantity;
            }
        }

        public double TotalMoney()
        {
            var total = items.Sum(p => p._shopping_product.Price * p._shopping_quantity);

            return (double)total;
        }

        public void RemoveItem(int id)
        {
            items.RemoveAll(p => p._shopping_product.ProductId == id);
        }

        public int TotalQuantity()
        {
            return items.Sum(p => p._shopping_quantity);
        }

        public void ClearCart()
        {
            items.Clear();
        }
    }
}