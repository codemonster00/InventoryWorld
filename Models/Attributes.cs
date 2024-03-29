using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventoryWorld.Models
{
    public class Attributes
    {

        public Attributes()
        {
            Value = new List<string>();
        }

        [Key]
        public int Id { get; set; }
        [DisplayName("Attribute Name")]
        [Required]
       
        public  string AttributeName { get; set; }
        
        public List<string> Value  { get; set; }

        public enum StatusType
        {
            Active,
            Inactive,
        }


       // public virtual List<ProductAttribute> ProductAttributes { get; set; }= new List<ProductAttribute>();

        //[EnumDataType(typeof(StatusType))]
        public StatusType Status { get; set; }







        public void AddValue(string value)
        {
            if (!Value.Equals(null))
            {
                Value.Add(value);
            }
        }

        public void RemoveValue(string value)
        {
            if (!Value.Equals(null))
            {
                Value.Remove(value);
            }
            
        }

        public void EditValue(string value, string newvalue)
        {
            Value.Remove(value);
            Value.Add(newvalue);
        }

        public List<string> ReturnValues() { 
        
            
        return Value.ToList();
        }
    }
}
