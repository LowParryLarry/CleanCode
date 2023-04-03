namespace WebShopCleanCode.Menu;

public class MenuItem 
{
    public string Title { get; set; }
    public int? SubMenuId { get; init; }
    public Action Action { get; init; }
    public bool AuthenticationRequired { get; init; }
}

//Känns fel att detta är det enda i klassen, funderade på struct men då får man inte ändra värdena.. ?