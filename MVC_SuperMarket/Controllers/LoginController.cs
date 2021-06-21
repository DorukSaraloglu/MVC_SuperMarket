using MVC_SuperMarket.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC_SuperMarket.Controllers
{
    public class LoginController : BaseController
    {
        // GET: Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(User user)
        {
            Users users = context.Users.FirstOrDefault(u => u.UserName == user.UserName);

            if (users!=null)
            {
                HttpCookie userIdCookie = new HttpCookie("UserName");
                userIdCookie.Value = users.UserName;
                Response.Cookies.Add(userIdCookie);

                int cartId = GetCartId();
                var cartProduct = context.CartProducts.Where(p => p.CartId == cartId && p.IsActive == true).ToList();
                int quantity = 0;

                foreach (var item in cartProduct)
                {
                    quantity = quantity + item.Quantity;
                }

                HttpCookie cartCookie = new HttpCookie("Cart");
                cartCookie.Values["cart"] = quantity.ToString();
                Response.Cookies.Add(cartCookie);

                return RedirectToAction("Index", "Product");
            }
            else
            {
                ViewBag.Message = "Kullanıcı bilgileri bulunamadı!";
                return View();
            }
        }
    }
}