namespace WebShopCleanCode.WebShop;

public class Order
{
    private string Name { get; set; }
    private int BoughtFor { get; set; }
    private DateTime PurchaseTime { get; set; }
    public Order(string name, int boughtFor, DateTime purchaseTime)
    {
        Name = name;
        BoughtFor = boughtFor;
        PurchaseTime = purchaseTime;
    }
    public void PrintInfo()
    {
        Console.WriteLine($"{Name} bought for {BoughtFor}kr, time: {PurchaseTime}");
    }
}