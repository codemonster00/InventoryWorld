using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventoryWorld.Models
{
    public class Brands
    {
        public int Id { get; set; }
        [DisplayName("Brand Name")]       
        

        public  string BrandName { get; set; } = string.Empty;



       
        public enum StatusType { 
            Active,
            Inactive,
        }

        //[EnumDataType(typeof(StatusType))]
        public StatusType Status { get; set; }

    }
}
