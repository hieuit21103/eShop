namespace Order.API.Models
{
    public class CreateOrderResult
    {
        public CustomerOrder Order { get; set; }
        public bool Success => string.IsNullOrEmpty(Error);
        public string Error { get; set; }
        public static CreateOrderResult Fail(string error) => new CreateOrderResult { Error = error };
        public static CreateOrderResult Ok(CustomerOrder order) => new CreateOrderResult { Order = order };
    }

}