//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DoAnCuoiKy.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.OrderItems = new HashSet<OrderItem>();
        }
    
        public string pro_id { get; set; }
        public string cat_id { get; set; }
        public string pro_name { get; set; }
        public string quantity_perUnit { get; set; }
        public int unit_price { get; set; }
        public int units_instock { get; set; }
        public int units_onOrder { get; set; }
        public string pro_status { get; set; }
        public string product_img { get; set; }
    
        public virtual Category Category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual CartItem CartItem { get; set; }
    }
}
