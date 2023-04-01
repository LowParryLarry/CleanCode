namespace WebShopCleanCode.States;
using WebShop;
using Interfaces;
using Menu;


public class UnAuthenticatedState : ILoginState
{
    public void ExecuteMenuItem(MenuCollection menuCollection, MenuItem menuItemSelected)
    {
        if (menuItemSelected.AuthenticationRequired)
        {
            Console.WriteLine("\nNobody is logged in.\n");
            menuCollection.RunMenu(menuCollection.CurrentMenu.MenuId);
        }
        else
        {
            menuCollection.RunMenuItem(menuItemSelected);
        }
    }

    public void ToggleLoginState(WebShop webShop)
    {
        webShop.TryLogin();
    }
}