namespace WebShopCleanCode.Menu;

public class MenuItem
{
    public string Title { get; set; }
    public int? SubMenuId { get; init; }
    public Action Action { get; init; }
    public bool AuthenticationRequired { get; init; }
}