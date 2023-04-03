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

    /// <summary>
    /// Runs menu.
    /// </summary>
    /// <param name="id"></param>
    public void RunMenu(int id)
    {
        CurrentMenu = GetMenu(id);
        PrintMenuGetIndex();
        ProcessMenuItem(CurrentMenu.MenuItems[SelectedIndex]);
    }

    /// <summary>
    /// Prints menu to console. Exits while loop on Enter.
    /// </summary>
    private void PrintMenuGetIndex()
    {
        do
        {
            PrintMenuItems(CurrentMenu);
        } while (SetSelectedIndex(CurrentMenu) != ConsoleKey.Enter);
    }

    /// <summary>
    /// Prints menu and sets space-invader to selected menu item.
    /// </summary>
    /// <param name="currentMenu"></param>
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

    /// <summary>
    /// If current menu is Purchase menu, will display funds.
    /// </summary>
    private void PrintFundsIfPurchaseMenu()
    {
        if (CurrentMenu.Title != "Purchase Menu") return;
        var funds = WebShopInstance.CurrentCustomer.Funds;
        Console.WriteLine($"\nYour funds: {funds}");
    }
    
    private void ProcessMenuItem(MenuItem menuItemSelected)
    {
        WebShopInstance.State.ProcessMenuItem(this, menuItemSelected);
    }

    /// <summary>
    /// Sets the index of currently selected item.
    /// </summary>
    /// <param name="currentMenu"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Returns menu by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private Menu GetMenu(int id)
    {
        return Menus.Single(menu => menu.SubMenuId == id);
    }

    /// <summary>
    /// If menu item is Action, execute. If menu is sub menu, run sub menu.
    /// </summary>
    /// <param name="selectedMenuItem"></param>
    public void ExecuteOrNavigate(MenuItem selectedMenuItem)
    {
        SelectedIndex = 0;
        
        if (selectedMenuItem.SubMenuId.HasValue)
        {
            MenuHistory.Add(CurrentMenu.SubMenuId);
            RunMenu(selectedMenuItem.SubMenuId.Value);
        }
        else
        {
            selectedMenuItem.Action();
            RunMenu(CurrentMenu.SubMenuId);
        }
    }

    /// <summary>
    /// Runs previous menu.
    /// </summary>
    public void Back()
    {
        var previousMenu = MenuHistory.Last();
        
        MenuHistory.RemoveAt(MenuHistory.Count - 1);
        
        RunMenu(previousMenu);
    }
}