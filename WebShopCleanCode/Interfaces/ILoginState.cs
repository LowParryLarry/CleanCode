namespace WebShopCleanCode.Interfaces;
using Menu;
using WebShop;

public interface ILoginState
{
    void ProcessMenuItem(MenuCollection menuCollection, MenuItem selectedMenuItem);

    void ToggleLoginState(WebShop webShop);
}