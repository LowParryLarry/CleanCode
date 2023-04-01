namespace WebShopCleanCode.Menu;
using WebShop;

public class MenuCollection
{
        public List<Menu> Menus { get; } = new();
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

        PrintMenu();

        var selectedMenuItem = CurrentMenu.MenuItems[SelectedIndex];

        ExecuteMenuItem(selectedMenuItem);
    }

    private Menu GetMenu(int id)
    {
        return Menus.Single(menu => menu.MenuId == id);
    }

    private void PrintMenu()
    {
        do
        {
            PrintMenuItems(CurrentMenu);
        } while (SetSelectedIndex(CurrentMenu) != ConsoleKey.Enter);
        
    }

    private void PrintMenuItems(Menu currentMenu)
    {
        Console.CursorVisible = false;
        
        Console.WriteLine(currentMenu.Prompt);
        
        for (var i = 0; i < currentMenu.MenuItems.Count; i++)
        {
            if (i == SelectedIndex)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
            }

            Console.WriteLine($"{currentMenu.MenuItems[i].Title}"); //TODO {i + 1}:  
        }
        Console.ResetColor();
        
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

    private void ExecuteMenuItem(MenuItem menuItemSelected)
    {
        WebShopInstance.State.ExecuteMenuItem(this, menuItemSelected);
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

    public void RunMenuItem(MenuItem menuItemSelected)
    {
        SelectedIndex = 0;
        
        if (menuItemSelected.SubMenuId.HasValue)
        {
            MenuHistory.Add(CurrentMenu.MenuId);
            RunMenu(menuItemSelected.SubMenuId.Value);
        }
        else
        {
            menuItemSelected.Action();
            RunMenu(CurrentMenu.MenuId);
        }
    }

    public void Back()
    {
        var lastEntry = MenuHistory.Last();
        
        MenuHistory.RemoveAt(MenuHistory.Count - 1);
        
        RunMenu(lastEntry);
    }

    public static void Quit()
    {
        Console.WriteLine("\nThe console powers down. You are free to leave.");
        Environment.Exit(0);
    }
    
}