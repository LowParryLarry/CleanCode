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

    public List<Product> GetProducts()
    {
        return _productsInDatabase;
    }

    public List<Customer> GetCustomers()
    {
        return _customersInDatabase;
    }

    public bool UsernameExists(string username)
    {
        return _customersInDatabase.Any(user => user.CheckUsername(username));
    }

    public void AddNewCustomer(Customer newCustomer)
    {
        _customersInDatabase.Add(newCustomer);
    }   
}