using MVC_SuperMarket.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_SuperMarket.Controllers
{
    public class ProductController : BaseController
    {
        // GET: Product
        public ActionResult Index()
        {
            List<Products> products = context.Products.ToList();
            return View(products);
        }

        [HttpGet]
        public ActionResult GetProduct(int id)
        {
            var product = context.Products.FirstOrDefault(p => p.ProductId == id);
            ViewBag.Product = product;
            ViewBag.Category = context.Categories.FirstOrDefault(c => c.CategoryId == product.CategoryId);

            return RedirectToAction("ProductUpdate");
        }

        public ActionResult ProductAdd()
        {
            ViewBag.Categories = context.Categories.ToList();

            return View();
        }


        [HttpPost]
        public ActionResult ProductAdd(Products product)
        {
            context.Products.Add(product);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ProductUpdate(int id)
        {
            var product = context.Products.FirstOrDefault(p => p.ProductId == id);
            ViewBag.Product = product;
            ViewBag.Category = context.Categories.FirstOrDefault(c => c.CategoryId == product.CategoryId);
            ViewBag.Categories = context.Categories.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult ProductUpdate(ProductUpdate product)
        {
            var model = context.Products.FirstOrDefault(p => p.ProductId == product.ProductId);
            model.ProductName = product.ProductName;
            model.Price = product.Price;
            model.StockAmount = product.StockAmount;
            model.CategoryId= product.CategoryId;
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ProductRemove(int? id)
        {
            Products product = context.Products.FirstOrDefault(x => x.ProductId == id);
            context.Products.Remove(product);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}