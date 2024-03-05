namespace RestaurantAPI.Models;

public class OrderMaster
{
    public long OrderMasterId { get; set; }
    public string? OrderNumber { get; set; }
    public string? PaymentMethod { get; set; }
    public decimal GrandTotal { get; set; }

    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public List<OrderDetail>? OrderDetails { get; set; }
    public string? DeletedOrderItemIds { get; set; }
}