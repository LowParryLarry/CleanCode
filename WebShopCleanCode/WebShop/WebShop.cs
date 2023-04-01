using WebShopCleanCode.Static;

namespace WebShopCleanCode.WebShop;

using SortingAlgorithms;
using System.Text.RegularExpressions;
using Interfaces;
using Menu;
using States;
using System.Reflection;

public class WebShop
{
    public Customer CurrentCustomer { get; set; }
    private string TempUsername { get; set; }
    private string TempPassword { get; set; }
    public ILoginState State { get; set; }
    private MenuCollection MenuCollection { get; set; }
    private Database Database { get; set; }
    public List<Product> Products { get; set; }
    private List<Customer> Customers { get; set; }
    public WebShop()
    {
        State = new UnAuthenticatedState();
        Database = new Database();
        Products = Database.GetProducts();
        Customers = Database.GetCustomers();
        MenuCollection = CreateMenuCollection();
        TempUsername = "jake"; //string.Empty;
        TempPassword = "jake123"; //string.Empty;
    }

    private enum UserDataType
    {
        Username,
        Password
    }
    
    private MenuCollection CreateMenuCollection()
    {//TODO command?
        return new MenuCollection(this)
        {
            Menus =
            {
                new Menu
                {
                    Title = "Main Menu",
                    Prompt = "What would you like to do?",
                    MenuId = 0,
                    MenuItems =
                    {
                        new MenuItem
                        {
                            Title = "See Wares",
                            SubMenuId = 1
                        },
                        new MenuItem
                        {
                            Title = "Customer Info",
                            SubMenuId = 4,
                            AuthenticationRequired = true
                        },
                        new MenuItem
                        {
                            Title = "Login",
                            SubMenuId = 2
                        },
                        new MenuItem
                        {
                            Title = "Quit",
                            Action = Utility.Quit
                        }
                    }
                },
                new Menu
                {
                    Title = "Wares Menu",
                    Prompt = "What would you like to do?",
                    MenuId = 1,
                    MenuItems =
                    {
                        new MenuItem
                        {
                            Title = "See all wares",
                            Action = PrintAllWares
                        },
                        new MenuItem
                        {
                            Title = "Purchase a ware",
                            SubMenuId = 5,
                            AuthenticationRequired = true
                        },
                        new MenuItem
                        {
                            Title = "Sort wares",
                            SubMenuId = 3
                        },
                        new MenuItem
                        {
                            Title = "Login",
                            SubMenuId = 2
                        },
                        new MenuItem
                        {
                            Title = "Back",
                            Action = () => MenuCollection.Back()
                        }
                    }
                },
                new Menu
                {
                    Title = "Login Menu",
                    Prompt = "Please submit username and password.",
                    MenuId = 2,
                    MenuItems =
                    {
                        new MenuItem
                        {
                            Title = "Set Username",
                            Action = () => SetCredentials("Please input your username.", UserDataType.Username)
                        },
                        new MenuItem
                        {
                            Title = "Set Password",
                            Action = () => SetCredentials("Please input your passowrd.", UserDataType.Password)
                        },
                        new MenuItem
                        {
                            Title = "Login",
                            Action = ToggleLoginStatus
                        },
                        new MenuItem
                        {
                            Title = "Register",
                            Action = RegisterUser
                        },
                        new MenuItem
                        {
                            Title = "Back",
                            Action = () => MenuCollection.Back()
                        }
                    }
                },
                new Menu
                {
                    Title = "Sort Menu",
                    Prompt = "How would you like to sort them?",
                    MenuId = 3,
                    MenuItems =
                    {
                        new MenuItem
                        {
                            Title = "Sort by name, descending",
                            Action = () => QuickSort.Sort(Products, "Name",false)
                        },
                        new MenuItem
                        {
                            Title = "Sort by name, ascending",
                            Action = () => QuickSort.Sort(Products, "Name",true)
                        },
                        new MenuItem
                        {
                            Title = "Sort by price, descending",
                            Action = () => QuickSort.Sort(Products, "Price",false)
                        },
                        new MenuItem
                        {
                            Title = "Sort by price, ascending",
                            Action = () => QuickSort.Sort(Products, "Price",true)
                        },
                        new MenuItem
                        {
                            Title = "Back",
                            Action = () => MenuCollection.Back()
                        }
                    }
                },
                new Menu
                {
                    Title = "Customer Menu",
                    Prompt = "What would you like to do?",
                    MenuId = 4,
                    MenuItems =
                    {
                        new MenuItem
                        {
                            Title = "See your orders",
                            Action = () => CurrentCustomer.PrintOrders()
                        },
                        new MenuItem
                        {
                            Title = "Set your info",
                            Action = () => CurrentCustomer.PrintInfo()
                        },
                        new MenuItem
                        {
                            Title = "Add funds",
                            Action = () => CurrentCustomer.AddFunds()
                        },
                        new MenuItem
                        {
                            Title = "Back",
                            Action = () => MenuCollection.Back()
                        }
                    }
                },
                new Menu(this)
                {
                    Title = "Purchase Menu",
                    Prompt = "What would you like to purchase?",
                    MenuId = 5,
                    MenuItems =
                    {
                        new MenuItem
                        {
                            Title = "Back",
                            Action = () => MenuCollection.Back()
                        }
                    }
                }
            }
        };
    }

    public void Run()
    {
        MenuCollection.RunMenu(MenuCollection.SelectedIndex);
    }

    private void PrintAllWares()
    {
        Utility.PrintEmptyLine();
        foreach (var item in Products) item.PrintInfo();
        Utility.PrintEmptyLine();
    }

    public void PrintCurrentUser()
    {
        Console.WriteLine(CurrentCustomer == null
            ? "Nobody logged in.\n"
            : $"Current user: {CurrentCustomer.Username}\n");
    }

    private void SetCredentials(string promt, UserDataType userDataType)
    {
        Console.CursorVisible = true;
        
        Console.WriteLine("A keyboard appears.");
        Console.WriteLine(promt);

        switch (userDataType)
        {
            case UserDataType.Username:
                TempUsername = Console.ReadLine();
                break;
            case UserDataType.Password:
                TempPassword = Console.ReadLine();
                    break;
        }

        Utility.PrintEmptyLine();
    }

    private void SetState(ILoginState state)
    {
        State = state;
    }

    private void ToggleLoginStatus()
    {   
        State.ToggleLoginState(this);
    }

    private static void PrintAuthSuccess(Customer customer)
    {
        Console.WriteLine($"\n{customer.Username} logged in.\n");
    }

    private void ToggleLoginLogoutTitle()
    {
        foreach (var menu in MenuCollection.Menus)
        {//TODO Register ska inte finnas om inloggad
            foreach (var menuItem in menu.MenuItems)
            {
                if (menuItem.Title is "Login" or "Logout")
                {
                    menuItem.Title = menuItem.Title == "Login" ? "Logout" : "Login";
                }
            }
        }
    }

    private void RegisterUser()
    {
        Customer customer = new();
        
        SetUsername(customer);
        SetProperties(customer);
        Database.AddNewCustomer(customer);
        
        TryLogin();
    }

    private static void SetUsername(Customer customer)
    {
        Console.CursorVisible = true;
        Console.WriteLine("Please write your username.");
        customer.Username = Console.ReadLine();
    }

    private void SetProperties(Customer customer)
    {
        var properties = GetCustomerProperties();

        foreach (var property in properties)
        {
            if (PropertyExclusion(property))
            {
                continue;
            }
            
            var determiner = GetDeterminer(property.Name);
            var userInput = UserInput(determiner, property.Name);
            
            if (userInput != null)
            {
                property.SetValue(customer, ConvertIfInt(userInput));
            }
        }

        TempUsername = customer.GetUsername();
        TempPassword = customer.GetPassword();
    }

    private static IEnumerable<PropertyInfo> GetCustomerProperties()
    {
        return typeof(Customer).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    }

    private static bool PropertyExclusion(PropertyInfo property)
    {
        return property.PropertyType.IsGenericType &&
               property.PropertyType.GetGenericTypeDefinition() == typeof(List<>) ||
               property.Name is "Funds" or "Username";
    }

    private static string FormatPropertyName(string input)
    {
        var splitString = Regex.Replace(input, "([A-Z])", " $1");

        var lowerCaseString = splitString.ToLower();

        return lowerCaseString;
    }

    private static object UserInput(string determiner, string propertyName)
    {
        var formattedProperyName = FormatPropertyName(propertyName);
        
        const string ynError = "y or n, please.\n";

        while (true)
        {
            Console.WriteLine($"Do you want {determiner}{formattedProperyName}? y/n");
            Console.CursorVisible = true;
            
            var ynChoice = Console.ReadLine();
            
            switch (ynChoice)
            {
                case "n":
                    return null;
                case "y":
                    return CheckForInputError(formattedProperyName);
                case "":
                    Console.WriteLine(ynError);
                    break;
            }
        }
    }

    private static object ConvertIfInt(object inputString) 
    {
        if (int.TryParse(inputString.ToString(), out var intValue))
        {
            return intValue;
        }

        return inputString;
    }

    private static string CheckForInputError(string propertyName)
    {
        const string inputError = "Please actually write something.";
        string input;

        while (true)
        {
            Console.WriteLine($"Please enter your {propertyName}.");
            Console.CursorVisible = true;
        
            input = Console.ReadLine();

            if (input != string.Empty)
            {
                break;
            }

            if (input == string.Empty)
            {
                Console.WriteLine(inputError);
            }
            
        }

        return input;
    }

    private static string GetDeterminer(string word)
    {
        if (string.IsNullOrEmpty(word)) return string.Empty;

        var firstLetter = char.ToLower(word[0]);
        char[] vowels = { 'a', 'e', 'i', 'o', 'u' };

        return Array.IndexOf(vowels, firstLetter) >= 0 ? "an" : "a";
    }

    public void TryLogin()
    {
        var customer = CheckCredentials(TempUsername, TempPassword);

        if (customer == null)
        {
            Console.WriteLine("Invalid credentials.\n");
            return;
        }

        LoginProcess(customer);
    }

    private void LoginProcess(Customer customer)
    {
        PrintAuthSuccess(customer);
        CurrentCustomer = customer;
        ToggleLoginLogoutTitle();
        SetState(new AuthenticatedState());
    }

    private Customer CheckCredentials(string username, string password)
    {
        return Customers.FirstOrDefault(c =>
            c.CheckUsername(username) && c.CheckPassword(password));
    }

    public void Logout()
    {
        Console.WriteLine($"\n{CurrentCustomer.Username} logged out\n");
        CurrentCustomer = null;
        ToggleLoginLogoutTitle();
        SetState(new UnAuthenticatedState());
        ResetTempData();
    }

    public static void WithdrawFundsFromCustomer(Customer currentCustomer, Product product)
    {
        if (!product.ProductIsInStock())
        {
            Console.WriteLine($"Not in stock.\n");
            return;
        }

        if (!currentCustomer.CanAfford(product.Price))
        {
            Console.WriteLine($"You cannot afford.\n");
            return;
        }
        
        currentCustomer.Funds -= product.Price;
        currentCustomer.AddOrder(product.Name, product.Price);
        product.NrInStock--;

        Console.WriteLine($"Successfully bought {product.Name}\n");
    }

    private void ResetTempData()
    {
        TempUsername = string.Empty;
        TempPassword = string.Empty;
    }
}