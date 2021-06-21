using MVC_SuperMarket.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_SuperMarket.Controllers
{
    public class CartController : BaseController
    {
        // GET: Cart
        public ActionResult Index()
        {
            int cartId = GetCartId();

            List<CartProducts> cartProducts = context.CartProducts.Where(p => p.CartId == cartId && p.IsActive == true).ToList();

            List<Product> productList = new List<Product>();

            foreach (var item in cartProducts)
            {
                Products product = context.Products.FirstOrDefault(p => p.ProductId == item.ProductId);
                productList.Add(new Product
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Price = product.Price * item.Quantity,
                    CategoryName = context.Categories.FirstOrDefault(c => c.CategoryId == product.CategoryId).CategoryName,
                    Quantity = item.Quantity

                });
            }
            ViewBag.PaymentTypes = context.PaymentTypes;



            return View(productList);
        }

        [HttpPost]
        public void CartAdd(int id)
        {
            int userId = GetUserId();
            int cartId = GetCartId();

            int quantity = 1;

            if (cartId == 0)
            {
                context.Carts.Add(new Carts
                {
                    UserId = userId
                });
            }

            CartProducts cartProduct = context.CartProducts.FirstOrDefault(p => p.ProductId == id && p.CartId == cartId && p.IsActive == true);

            if (cartProduct == null)
            {
                context.CartProducts.Add(new CartProducts
                {
                    Quantity = quantity,
                    ProductId = id,
                    CartId = cartId,
                    IsActive = true
                });
            }
            else
            {
                var product = context.CartProducts.First(p => p.ProductId == id && p.IsActive == true);
                product.Quantity = product.Quantity + quantity;
                context.SaveChanges();
            }

            Products productsStock = context.Products.FirstOrDefault(s => s.ProductId == id);
            productsStock.StockAmount = productsStock.StockAmount - quantity;

            context.SaveChanges();

            HttpCookie cookie = Request.Cookies["Cart"];
            int CartQuantity = int.Parse(cookie.Values["cart"]) + 1;
            cookie.Values["cart"] = CartQuantity.ToString();

            Response.Cookies.Add(cookie);
        }

        [HttpPost]
        public void CartRemove(int id)
        {
            int cartId = GetCartId();

            CartProducts cartProduct = context.CartProducts.FirstOrDefault(p => p.ProductId == id && p.CartId == cartId && p.IsActive == true);

            Products productsStock = context.Products.FirstOrDefault(s => s.ProductId == id);
            
            if (cartProduct.Quantity == 1)
            {
                context.CartProducts.Remove(cartProduct);
                context.SaveChanges();
            }
            else
            {
                cartProduct.Quantity = cartProduct.Quantity - 1;
                productsStock.StockAmount = productsStock.StockAmount + 1;
                context.SaveChanges();
            }

            context.SaveChanges();

            HttpCookie cookie = Request.Cookies["Cart"];
            int CartQuantity = int.Parse(cookie.Values["cart"]) - 1;
            cookie.Values["cart"] = CartQuantity.ToString();

            Response.Cookies.Add(cookie);

        }

        public ActionResult SalesComplete(Sale sale)
        {
            int totalAmount = 0;

            int cartId = GetCartId();

            var cartProduct = context.CartProducts.Where(p => p.CartId == cartId && p.IsActive);

            foreach (var item in cartProduct)
            {
                int price = context.Products.FirstOrDefault(a => a.ProductId == item.ProductId).Price;
                totalAmount = totalAmount + (item.Quantity * price);
                item.IsActive = false;
            }

            if (cartProduct.ToList().Count >0)
            {

            context.Sales.Add(new Sales
            {
                CartId = cartId,
                PaymentTypeId = sale.PaymentTypeId,
                SalesDate = DateTime.Now,
                TotalAmount = totalAmount
            });

            context.SaveChanges();

            HttpCookie cookie = Request.Cookies["Cart"];
            int CartQuantity = 0;
            cookie.Values["cart"] = CartQuantity.ToString();

            Response.Cookies.Add(cookie);
                return RedirectToAction("Index", "Product");

            }
            else
            {
                return RedirectToAction("Index", "Cart");
            }
        }
    }
}