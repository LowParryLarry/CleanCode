namespace WebShopCleanCode.Static;
    
public static class Utility
{
    /*
     * Läste någonstans att detta var ett bra tillvägagångssätt men
     * lyckades inte få in så mycket här :)
     */
    public static void Quit()
    {
        Console.WriteLine("\nThe console powers down. You are free to leave.");
        Environment.Exit(0);
    }

    public static void PrintEmptyLine()
    {
        Console.WriteLine();
    }

}