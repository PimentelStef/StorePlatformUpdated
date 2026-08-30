namespace OrderService.Models
{
    public class OrderInput
    {
        public string CustomerName { get; set; } = "";
        public List<OrderItemInput> Items { get; set; } = new();
    }
}