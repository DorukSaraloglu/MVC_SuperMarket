using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_SuperMarket.Models.Classes
{
    public class ProductUpdate
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public int StockAmount { get; set; }
        public int Price { get; set; }
    }
}