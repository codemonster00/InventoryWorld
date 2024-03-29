using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing.Drawing2D;

namespace InventoryWorld.Models
{

  
    public class Products
    {

   
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public double Price { get; set; }

        public string Sku { get; set; } = string.Empty;

        public int Quantity { get; set; }



        [ForeignKey("Brands")]
        public int BrandId { get; set; }
        public Brands Brand { get; set; } = new Brands();
      

        [ForeignKey("Store")]
        public int StoreId { get; set; }
        public Stores Store { get; set; } = new Stores();

        public ICollection<ProductAttribute> ProductAttributes { get; set; } = new List<ProductAttribute>();

        public bool Availability { get; set; }
    }
}
