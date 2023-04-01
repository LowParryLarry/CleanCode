namespace WebShopCleanCode.Menu;
using WebShop;

public class Menu
{
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
    
    private WebShop WebShopInstance { get; set; }

    public string Prompt { get; init; }
    
    public int MenuId { get; init; }
    
    public List<MenuItem> MenuItems { get; set; }

    public string Title { get; set; }

    private void ConvertProductsToMenuItems()
    {
        foreach (var product in WebShopInstance.Products)
        {
            MenuItems.Add(new MenuItem
            {
                Title = $"{product.Name}, {product.Price}",
                Action = () => WebShop.WithdrawFundsFromCustomer(WebShopInstance.CurrentCustomer, product)
            });
            
        }
    }
}