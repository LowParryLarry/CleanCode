namespace WebShopCleanCode.States;
using WebShop;
using Interfaces;
using Menu;

public class AuthenticatedState : ILoginState
{
    public void ExecuteMenuItem(MenuCollection menuCollection, MenuItem menuItemSelected)
    {
        menuCollection.RunMenuItem(menuItemSelected);
    }

    public void ToggleLoginState(WebShop webShop)
    {
        webShop.Logout();
    }
}