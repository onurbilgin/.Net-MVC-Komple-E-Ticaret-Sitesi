//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace iakademi5_proje.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_Orders
    {
        public int OrderID { get; set; }
        public Nullable<int> userID { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public string orderGroupGUID { get; set; }
        public string invoiceAddress { get; set; }
        public Nullable<int> ProductID { get; set; }
        public Nullable<int> quantity { get; set; }
        public Nullable<bool> aktif { get; set; }
    }
}
