using WebShopCleanCode.Static;

namespace WebShopCleanCode.WebShop;

public class Customer
{
    public string Username { get; set; }
    private string Password { get; set; }
    private string FirstName { get; set;}
    private string LastName { get; set;}
    private string Email { get; set;}
    private int Age { get; set;}
    private string Address { get;set; }
    private string PhoneNumber { get; set;}
    public int Funds { get; set; }
    private List<Order> Orders { get; }

    public Customer(
        string username, string password, string firstName,
        string lastName, string email, int age,
        string address, string phoneNumber)
    {
        Username = username;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Age = age;
        Address = address;
        PhoneNumber = phoneNumber;
        Orders = new List<Order>();
        Funds = 0;
    }
    
    public Customer()
    {
        Username = string.Empty; 
        Password = string.Empty;
        FirstName = string.Empty;
        LastName = string.Empty;
        Email = string.Empty;
        Age = 0;
        Address = string.Empty;
        PhoneNumber = string.Empty;
        Orders = new List<Order>(); 
        Funds = 0;
    }

    /// <summary>
    /// Returns true if funds exeed the given price.
    /// </summary>
    /// <param name="price"></param>
    /// <returns></returns>
    public bool CanAfford(int price)
    {
        return Funds >= price;
    }

    /// <summary>
    /// Returns true if given password equals actual password.
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public bool CheckPassword(string password)
    {
        return password.Equals(Password);
    }

    /// <summary>
    /// Returns true if given username equals actual username.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public bool CheckUsername(string username)
    {
        return username.Equals(Username);
    }
    
    /// <summary>
    /// Returns password.
    /// </summary>
    /// <returns></returns>
    public string GetPassword()
    {
        return Password;
    }
    
    /// <summary>
    /// Returns username
    /// </summary>
    /// <returns></returns>
    public string GetUsername()
    {
        return Username;
    }

    /// <summary>
    /// Prints summary of customer.
    /// </summary>
    public void PrintInfo()
    {
        Console.Write("\nUsername: " + Username + "");
        if (Password != null) Console.Write(", Password: " + Password);
        if (FirstName != null) Console.Write(", First Name: " + FirstName);
        if (LastName != null) Console.Write(", Last Name: " + LastName);
        if (Email != null) Console.Write(", Email: " + Email);
        if (Age != -1) Console.Write(", Age: " + Age);
        if (Address != null) Console.Write(", Address: " + Address);
        if (PhoneNumber != null) Console.Write(", Phone Number: " + PhoneNumber);
        Console.WriteLine(", Funds: " + Funds +"\n");
    }

    /// <summary>
    /// Prints orders of customer.
    /// </summary>
    public void PrintOrders()
    {
        Utility.PrintEmptyLine();
        foreach (var order in Orders) order.PrintInfo();
        Utility.PrintEmptyLine();
    }

    /// <summary>
    /// Adds new order for customer.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="price"></param>
    public void AddOrder(string name, int price)
    {
        Orders.Add(new Order(name, price, DateTime.Now));
    }

    /// <summary>
    /// Adds given funds to customer.
    /// </summary>
    public void AddFunds()
    {
        Console.CursorVisible = true;
        Console.WriteLine("How many funds would you like to add?");

        var input = Console.ReadLine(); 

        if (!int.TryParse(input, out var amount) || input == string.Empty)
        {
            Console.WriteLine("\nPlease write a number next time.\n");
            return;
        }

        if (amount < 0)
        {
            Console.WriteLine("Don't add negative amounts.");
            return;
        }

        Funds += amount;

        Console.WriteLine($"\n{amount} added to your profile.\n");
    }
}