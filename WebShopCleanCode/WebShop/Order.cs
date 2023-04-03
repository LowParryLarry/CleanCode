namespace WebShopCleanCode.WebShop;

public class Order
{
    private string Name { get; set; }
    private int Price { get; set; }
    private DateTime PurchaseTime { get; set; }
    public Order(string name, int price, DateTime purchaseTime)
    {
        Name = name;
        Price = price;
        PurchaseTime = purchaseTime;
    }
    
    /// <summary>
    /// Prints name, price and purchase date/time.
    /// </summary>
    public void PrintInfo()
    {
        Console.WriteLine($"{Name} bought for {Price}kr, time: {PurchaseTime}");
    }
}