using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InventoryWorld.Models
{
    public class ProductAttribute
    {
      
        [Key]
        public int ProductAttributeId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Products Product { get; set; }= new Products();

        [ForeignKey("Attribute")]
        public int AttributeId { get; set; }
        public Attributes Attribute { get; set; }= new Attributes();

        public string Value { get; set; } = string.Empty;

    }
}