namespace WebShopCleanCode.Interfaces;
using Menu;
using WebShop;

public interface ILoginState
{
    void ExecuteMenuItem(MenuCollection menuCollection, MenuItem menuItemSelected);

    void ToggleLoginState(WebShop webShop);
}