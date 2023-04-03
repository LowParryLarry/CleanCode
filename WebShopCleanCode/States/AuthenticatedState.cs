namespace WebShopCleanCode.States;
using WebShop;
using Interfaces;
using Menu;

public class AuthenticatedState : ILoginState
{
    public void ProcessMenuItem(MenuCollection menuCollection, MenuItem selectedMenuItem)
    {
        menuCollection.ExecuteOrNavigate(selectedMenuItem);
    }

    public void ChangeLoginState(WebShop webShop)
    {
        webShop.Logout();
    }
}