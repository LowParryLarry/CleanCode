namespace WebShopCleanCode.Builders;
using WebShop;
public class CustomerBuilder
{
    private string Username { get; set; } = string.Empty;
    private string Password { get; set; } = string.Empty;
    private string FirstName { get; set; } = string.Empty;
    private string LastName { get; set; } = string.Empty;
    private string Email { get; set; } = string.Empty;
    private int Age { get; set; }
    private string Address { get; set; } = string.Empty;
    private string PhoneNumber { get; set; } = string.Empty;

    public CustomerBuilder WithUsername(string username)
    {
        Username = username;
        return this;
    }

    public CustomerBuilder WithPassword(string password)
    {
        Password = password;
        return this;
    }

    public CustomerBuilder WithFirstName(string firstName)
    {
        FirstName = firstName;
        return this;
    }

    public CustomerBuilder WithLastName(string lastName)
    {
        LastName = lastName;
        return this;
    }

    public CustomerBuilder WithEmail(string email)
    {
        Email = email;
        return this;
    }

    public CustomerBuilder WithAge(int age)
    {
        Age = age;
        return this;
    }

    public CustomerBuilder WithAddress(string address)
    {
        Address = address;
        return this;
    }

    public CustomerBuilder WithPhoneNumber(string phoneNumber)
    {
        PhoneNumber = phoneNumber;
        return this;
    }

    public Customer Build()
    {
        return new Customer(Username, Password, FirstName, LastName, Email, Age, Address, PhoneNumber);
    }
    
}