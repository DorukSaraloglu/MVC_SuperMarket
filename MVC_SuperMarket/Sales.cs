//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVC_SuperMarket
{
    using System;
    using System.Collections.Generic;
    
    public partial class Sales
    {
        public int SalesId { get; set; }
        public int PaymentTypeId { get; set; }
        public int TotalAmount { get; set; }
        public System.DateTime SalesDate { get; set; }
        public int CartId { get; set; }
    
        public virtual Carts Carts { get; set; }
        public virtual PaymentTypes PaymentTypes { get; set; }
    }
}
