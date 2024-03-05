namespace RestaurantAPI.Models;

public class OrderDetail
{
    public long OrderDetailId { get; set; }
    public decimal FoodItemPrice { get; set; }
    public int Quantity { get; set; }

    public int FoodItemId { get; set; }
    public FoodItem? FoodItem { get; set; }

    public long OrderMasterId { get; set; }
}