namespace InventoryWorld.Models
{
    public class Company
    {


        public int Id { get; set; }
        public string Name { get; set; }
        public string ServiceCharge { get; set; }

        public string VatCharge { get; set; }

        public string Address {  get; set; }

        public string Phone { get; set; }

        public string Country { get; set; }
        public string Currency { get; set; }
    }

}
