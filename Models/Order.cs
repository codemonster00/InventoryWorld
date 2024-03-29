namespace InventoryWorld.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItem> OrderItems { get; set; } // List of products in the order

        public string CustomerName { get; set; }// Other order-related properties (e.g., customer info, total amount, etc.)

        public string PhoneNumber { get; set; }

        public bool PaymentStatus { get; set; }


        public Order()
        {
            OrderItems = new List<OrderItem>();
        }
    }

    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int ProductId { get; set; }
        // Reference to the product
        public Products Product { get; set; }
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount => Quantity * Rate; // Calculated property for the item amount

        // Other item-related properties (e.g., discounts, taxes, etc.)
    }


}
