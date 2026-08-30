namespace OrderService.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; } = "";
        public List<OrderItem> Items { get; set; } = new();
    }
}