using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace InventoryWorld.Models
{
    public class Stores
    {
        public int Id { get; set; }
        [DisplayName("Store Name")]

        public string StoresName { get; set; }= string.Empty;
        public enum StatusType
        {
            Active,
            Inactive,
        }
        [AllowNull]
        
        public ICollection<Products> Products { get; set; }= new List<Products>();
        //[EnumDataType(typeof(StatusType))]
        public StatusType Status { get; set; }
    }
}
