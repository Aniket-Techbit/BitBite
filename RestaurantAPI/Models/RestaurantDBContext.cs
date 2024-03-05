using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RestaurantAPI.Models;

public class RestaurantDBContext : DbContext
{
    public RestaurantDBContext(DbContextOptions<RestaurantDBContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<FoodItem>(ConfigureFoodItem);
        modelBuilder.Entity<OrderDetail>(ConfigureOrderDetail);
        modelBuilder.Entity<OrderMaster>(ConfigureOrderMaster);
        modelBuilder.Entity<Customer>(ConfigureCustomer);
    }

    private static void ConfigureFoodItem(EntityTypeBuilder<FoodItem> builder)
    {
        builder.HasKey(f => f.FoodItemId);
        builder.Property(f => f.FoodItemName).HasColumnType("nvarchar(100)");
        builder.Property(f => f.Price).HasColumnType("decimal(18,2)");
    }

    private static void ConfigureOrderDetail(EntityTypeBuilder<OrderDetail> builder)
    {
        builder.HasKey(od => od.OrderDetailId);
        builder.Property(od => od.FoodItemPrice).HasColumnType("decimal(18,2)");
    }

    private static void ConfigureOrderMaster(EntityTypeBuilder<OrderMaster> builder)
    {
        builder.HasKey(om => om.OrderMasterId);
        builder.Property(om => om.OrderNumber).HasColumnType("nvarchar(75)");
        builder.Property(om => om.PaymentMethod).HasColumnType("nvarchar(10)");
        builder.Property(om => om.GrandTotal).HasColumnType("decimal(18,2)");

        builder.Ignore(om => om.DeletedOrderItemIds);
    }

    private static void ConfigureCustomer(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.CustomerId);
        builder.Property(c => c.CustomerName).HasColumnType("nvarchar(100)");
    }


    public DbSet<Customer> Customers { get; set; }
    public DbSet<OrderMaster> OrderMasters { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<FoodItem> FoodItems { get; set; }
}