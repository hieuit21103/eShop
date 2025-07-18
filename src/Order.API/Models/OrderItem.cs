namespace Order.API.Models;

public class OrderItem
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public string Name { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }

    [ForeignKey("OrderId")]
    [JsonIgnore]
    public CustomerOrder Order { get; set; }
    
    public OrderItem(Guid id, Guid orderId, Guid productId, string name, decimal unitPrice, int quantity)
    {
        Id = id;
        OrderId = orderId;
        ProductId = productId;
        Name = name;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }
}