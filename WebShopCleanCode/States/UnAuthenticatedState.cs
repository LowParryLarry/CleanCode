namespace WebShopCleanCode.States;
using WebShop;
using Interfaces;
using Menu;


public class UnAuthenticatedState : ILoginState
{
    public void ProcessMenuItem(MenuCollection menuCollection, MenuItem selectedMenuItem)
    {
        if (selectedMenuItem.AuthenticationRequired)
        {
            Console.WriteLine("\nNobody is logged in.\n");
            menuCollection.RunMenu(menuCollection.CurrentMenu.SubMenuId);
        }
        else
        {
            menuCollection.ExecuteOrNavigate(selectedMenuItem);
        }
    }

    public void ChangeLoginState(WebShop webShop)
    {
        webShop.TryLogin();
    }
}