namespace WebShopCleanCode.Static;
    
public static class Utility
{
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