namespace WebShopCleanCode.WebShop;
using Builders;


public class Database
{
    private readonly List<Product> _productsInDatabase;
    private readonly List<Customer> _customersInDatabase;
    
    public Database()
    {
        _productsInDatabase = new List<Product>
        {
            new("Mirror", 300, 2),
            new("Car", 2000000, 2),
            new("Candle", 50, 2),
            new("Computer", 100000, 2),
            new("Game", 599, 2),
            new("Painting", 399, 2),
            new("Chair", 500, 2),
            new("Table", 1000, 2),
            new("Bed", 20000, 2)
        };
        

        _customersInDatabase = new List<Customer>
        {
            /*
            * Här har jag applicerat builder pattern för att bygga
            * kunder på ett snyggt och smidigt sätt. 
            */
            new CustomerBuilder()
                .WithUsername("jimmy")
                .WithPassword("jimisthebest")
                .WithFirstName("Jimmy")
                .WithLastName("Jamesson")
                .WithEmail("jj@mail.com")
                .WithAge(22)
                .WithAddress("Big Street 5")
                .WithPhoneNumber("123456789")
                .Build(),
            
            new CustomerBuilder()
                .WithUsername("jake")
                .WithPassword("jake123")
                .WithFirstName("Jake")
                .WithAge(0)
                .Build(),
        };
    }

    /// <summary>
    /// Returns a list of the products.
    /// </summary>
    /// <returns></returns>
    public List<Product> GetProducts()
    {
        return _productsInDatabase;
    }

    /// <summary>
    /// Returns a list of the customers.
    /// </summary>
    /// <returns></returns>
    public List<Customer> GetCustomers()
    {
        return _customersInDatabase;
    }

    /// <summary>
    /// Returns false if given string already exists.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public bool UsernameExists(string username)
    {
        /*
         * Denna funktion fanns i din kod men är inte implementerad på rätt sätt.
         * Dvs funktionen finns i koden men den fungerar inte. Jag har lagt till den
         * så att det inte kan existera två identiska användarnamn.
         */
        return _customersInDatabase.Any(user => user.CheckUsername(username));
    }

    /// <summary>
    /// Adds a customer object to database.
    /// </summary>
    /// <param name="newCustomer"></param>
    public void AddNewCustomer(Customer newCustomer)
    {
        _customersInDatabase.Add(newCustomer);
    }

}