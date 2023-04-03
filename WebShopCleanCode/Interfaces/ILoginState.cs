namespace WebShopCleanCode.Interfaces;
using Menu;
using WebShop;

public interface ILoginState
{
    /// <summary>
    /// Depending on state, menu item will be run.
    /// </summary>
    /// <param name="menuCollection"></param>
    /// <param name="selectedMenuItem"></param>
    void ProcessMenuItem(MenuCollection menuCollection, MenuItem selectedMenuItem);

    /// <summary>
    /// Depending on state, changes state.
    /// </summary>
    /// <param name="webShop"></param>
    void ChangeLoginState(WebShop webShop);
}