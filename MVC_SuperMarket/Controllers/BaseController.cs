using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_SuperMarket.Controllers
{
    public class BaseController : Controller
    {
        public SuperMarketDbEntities5 context = new SuperMarketDbEntities5();

        public int GetUserId()
        {
            HttpCookie userName = HttpContext.Request.Cookies.Get("UserName");
            return context.Users.FirstOrDefault(i => i.UserName == userName.Value).UserId;
        }

        public int GetCartId()
        {
            int userId = GetUserId();
            Carts cart = context.Carts.FirstOrDefault(c => c.UserId == userId);
            return cart.CartId;
        }
    }
}