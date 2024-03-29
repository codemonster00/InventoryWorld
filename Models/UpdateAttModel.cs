namespace InventoryWorld.Models
{
    public class UpdateAttModel
    {
        public int ?Id { get; set; }
        public string OldValue { get; set; } = string.Empty;

        public string NewValue { get; set; } = string.Empty;
    }
}
