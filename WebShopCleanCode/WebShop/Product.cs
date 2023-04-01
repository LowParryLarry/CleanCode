namespace WebShopCleanCode.WebShop;

using Menu;

public class Product : MenuItem
{
    public string Name { get; set; }
    public int Price { get; set; }
    public int NrInStock { get; set; }
    public Product(string name, int price, int nrInStock)
    {
        Name = name;
        Price = price;
        NrInStock = nrInStock;
    }
    public void PrintInfo()
    {
        Console.WriteLine($"{Name}: {Price}kr, {NrInStock} in stock.");
    }

    public bool ProductIsInStock()
    {
        return NrInStock > 0;
    }
}