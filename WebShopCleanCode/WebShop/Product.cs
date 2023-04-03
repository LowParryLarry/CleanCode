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
    
    /// <summary>
    /// Prints name, price and stock for current product.
    /// </summary>
    public void PrintInfo()
    {
        Console.WriteLine($"{Name}: {Price}kr, {NrInStock} in stock.");
    }

    /// <summary>
    /// Returns bool if product is in stock.
    /// </summary>
    /// <returns></returns>
    public bool ProductIsInStock()
    {
        return NrInStock > 0;
    }
}