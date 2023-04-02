namespace WebShopCleanCode.Menu;
using WebShop;


public class MenuCollection
{
    public List<Menu> Menus { get; set; } = new();
    public Menu CurrentMenu { get; set; }
    private List<int> MenuHistory { get; } = new();
    public int SelectedIndex { get; set; }
    private WebShop WebShopInstance { get; }

    public MenuCollection(WebShop webShop)
    {
        WebShopInstance = webShop;
        MenuHistory.Add(SelectedIndex);
    }
    
    public void RunMenu(int id)
    {
        CurrentMenu = GetMenu(id);
        PrintMenuGetIndex();
        ProcessMenuItem(CurrentMenu.MenuItems[SelectedIndex]);
    }

    private Menu GetMenu(int id)
    {
        return Menus.Single(menu => menu.Id == id);
    }

    private void PrintMenuGetIndex()
    {
        do
        {
            PrintMenuItems(CurrentMenu);
        } while (SetSelectedIndex(CurrentMenu) != ConsoleKey.Enter);
    }

    private void PrintMenuItems(Menu currentMenu)
    {
        Console.OutputEncoding = System.Text.Encoding.Unicode;
        Console.CursorVisible = false;
        Console.WriteLine(currentMenu.Prompt + "\n");

        const string whiteSpace = "   ";
        const string spaceInvader = "\ud83d\udc7e ";

        for (var i = 0; i < currentMenu.MenuItems.Count; i++)
        {
            var menuItem = currentMenu.MenuItems[i].Title;
            
            if (i == SelectedIndex)
            {
                Console.WriteLine($"{spaceInvader}{menuItem}");
            }
            else
            {
                Console.WriteLine($"{whiteSpace}{menuItem}");
            }
        }

        PrintFundsIfPurchaseMenu();
        Console.WriteLine("\nYour buttons are ↑ ↓, Enter to confirm.");
        WebShopInstance.PrintCurrentUser();
    }

    private void PrintFundsIfPurchaseMenu()
    {
        if (CurrentMenu.Title != "Purchase Menu") return;
        var funds = WebShopInstance.CurrentCustomer.Funds;
        Console.WriteLine($"Your funds: {funds}");
    }

    private void ProcessMenuItem(MenuItem menuItemSelected)
    {
        WebShopInstance.State.ProcessMenuItem(this, menuItemSelected);
    }

    private ConsoleKey SetSelectedIndex(Menu currentMenu)
    {
        var keyInfo = Console.ReadKey(true);
        var keyPressed = keyInfo.Key;
        
        switch (keyPressed)
        {
            case ConsoleKey.UpArrow:
            {
                SelectedIndex--;
                if (SelectedIndex == -1) SelectedIndex = currentMenu.MenuItems.Count - 1;
                break;
            }
            case ConsoleKey.DownArrow:
            {
                SelectedIndex++;
                if (SelectedIndex == currentMenu.MenuItems.Count) SelectedIndex = 0;
                break;
            }
                
        }

        return keyPressed;
    }

    public void ExecuteOrNavigate(MenuItem menuItemSelected)
    {
        SelectedIndex = 0;
        
        if (menuItemSelected.SubMenuId.HasValue)
        {
            MenuHistory.Add(CurrentMenu.Id);
            RunMenu(menuItemSelected.SubMenuId.Value);
        }
        else
        {
            menuItemSelected.Action();
            RunMenu(CurrentMenu.Id);
        }
    }

    public void Back()
    {
        var previousMenu = MenuHistory.Last();
        
        MenuHistory.RemoveAt(MenuHistory.Count - 1);
        
        RunMenu(previousMenu);
    }

    
}