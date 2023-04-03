namespace WebShopCleanCode.Menu;
using WebShop;

public class Menu
{
    private WebShop WebShopInstance { get; set; }
    public string Prompt { get; init; }
    public int SubMenuId { get; init; }
    public List<MenuItem> MenuItems { get; set; }
    public string Title { get; init; }
    
    public Menu()
    {
        MenuItems = new List<MenuItem>();
    }

    public Menu(WebShop webShop)
    {
        WebShopInstance = webShop;
        MenuItems = new List<MenuItem>();
        ConvertProductsToMenuItems();
    }
    
    /// <summary>
    /// Creates a list of items, converts the products to MenuItems to be able to
    /// choose between them in the menu.
    /// </summary>
    private void ConvertProductsToMenuItems()
    {
        foreach (var product in WebShopInstance.Products)
        {
            MenuItems.Add(new MenuItem
            {
                Title = $"{product.Name}, {product.Price}kr",
                Action = () => WebShopInstance.WithdrawFundsFromCustomer(WebShopInstance.CurrentCustomer, product)
            });
        }
    }
}