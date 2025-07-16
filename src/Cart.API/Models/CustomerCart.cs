namespace Cart.API.Models;

public class CustomerCart
{
    public string UserId { get; set; }
    public List<CartItem> Items { get; set; } = new List<CartItem>();

    public CustomerCart()
    {
    }

    public CustomerCart(string userId)
    {
        UserId = userId;
    }
}