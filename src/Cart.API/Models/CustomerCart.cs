namespace Cart.API.Models;

public class CustomerCart
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public List<CartItem> Items { get; set; } = [];
    public CustomerCart() { }
    
    public CustomerCart(string userId)
    {
        UserId = userId;
    }
    
}