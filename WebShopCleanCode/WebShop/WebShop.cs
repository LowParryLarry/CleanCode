namespace WebShopCleanCode.WebShop;

using Static;
using System.Text.RegularExpressions;
using Interfaces;
using Menu;
using States;
using System.Reflection;

/*
 * Har läst på lite om dependency injection under tiden. Enligt denna princip så
 * kanske jag borde vänt på pankakan. Att ha min WebShop i MenuCollection istället
 * för att ha MenuCollection i WebShop. Jag förstod dock inte detta helt vad som är bäst.
 */

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
        TempUsername = "jake"; 
        TempPassword = "jake123";
    }

    /// <summary>
    /// Used instead of string to avoid misspelling. 
    /// </summary>
    private enum UserDataType
    {
        Username,
        Password
    }
    
    /// <summary>
    /// Creates mapping of menu system.
    /// </summary>
    /// <returns></returns>
    private MenuCollection CreateMenuCollection()
    {
        /*
         * Skapar meny-strukturen. Vet inte om SubMenuId räknas som magic number.
         * Kom på i efterhand att jag borde lagt in SubMenu av typ Menu i MenuItem.
         * På så sätt hade inte SubMenuId behövts. Och man hade kunnat skriva en
         * rekursiv metod för att leta i meny-strukturen. Kom på detta för sent dock.
         */
        
        return new MenuCollection(this)
        {
            Menus =
            {
                new Menu
                {
                    Title = "Main Menu",
                    Prompt = "What would you like to do?",
                    SubMenuId = 0,
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
                    SubMenuId = 1,
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
                    SubMenuId = 2,
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
                    SubMenuId = 3,
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
                    SubMenuId = 4,
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
                    SubMenuId = 5,
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
    
    /// <summary>
    /// Initiates the WebShop and menus.
    /// </summary>
    public void Run()
    {
        Console.WriteLine("Welcome to the WebShop!");
        MenuCollection.RunMenu(MenuCollection.SelectedIndex);
    }
    
    /// <summary>
    /// Prints all wares to console.
    /// </summary>
    private void PrintAllWares()
    {
        Utility.PrintEmptyLine();
        foreach (var item in Products) item.PrintInfo();
        Utility.PrintEmptyLine();
    }

    /// <summary>
    /// Prints current customer if logged in to console.
    /// </summary>
    public void PrintCurrentUser()
    {
        if (CurrentCustomer == null)
        {
            Console.WriteLine("Nobody logged in.\n");
            return;
        }
        
        Console.WriteLine($"Current user: {CurrentCustomer.Username}\n");
    }
    
    /// <summary>
    /// Sets TempUsername and TempPassword for verification.
    /// </summary>
    /// <param name="promt"></param>
    /// <param name="userDataType"></param>
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

    /// <summary>
    /// Changes authentication state.
    /// </summary>
    /// <param name="state"></param>
    private void SetState(ILoginState state)
    {
        State = state;
    }
    
    /// <summary>
    /// Change login state.
    /// </summary>
    private void ToggleLoginStatus()
    {   
        //Hade lite svårt med namngivningen här.
        State.ChangeLoginState(this);
    }
    
    /// <summary>
    /// Succsessful login message to console.
    /// </summary>
    /// <param name="customer"></param>
    private static void PrintAuthSuccess(Customer customer)
    {
        Console.WriteLine($"\n{customer.Username} logged in.\n");
    }
    
    /// <summary>
    /// Changes "Login" to "Logout" and vice versa. Deletes Register menu on Login.
    /// </summary>
    private void ToggleMenuExeptions()
    {
        /*
         * En riktig ful-lösning. Jag hade byggt menysystemet innan jag insåg att det INTE
         * var en statisk meny. Kom inte på en lösning utan att typ behöva göra om allt.
         */
        foreach (var menu in MenuCollection.Menus.ToList())
        {
            foreach (var menuItem in menu.MenuItems.ToList())
            {
                if (menuItem.Title is "Login" or "Logout")
                {
                    menuItem.Title = menuItem.Title == "Login" ? "Logout" : "Login";
                }

                if (menuItem.Title == "Register")
                {
                    menu.MenuItems.Remove(menuItem);
                }
            }
        }
    }

    /// <summary>
    /// Adds new user to database and set user as authenticated.
    /// </summary>
    private void RegisterUser()
    {
        Customer customer = new();
        SetUsername(customer);
        SetProperties(customer);
        Database.AddNewCustomer(customer);
        TryLogin();
    }

    /// <summary>
    /// Sets temp username.
    /// </summary>
    /// <param name="customer"></param>
    private void SetUsername(Customer customer)
    {
        while (true)
        {
            Console.CursorVisible = true;
            Console.WriteLine("Please write your username.");
            string input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("\nPlease actually write something.\n");
            }
            else if (Database.UsernameExists(input))
            {
                Console.WriteLine("\nUsername already exists.\n");
            }
            else
            {
                customer.Username = input;
                return;
            }
        }
    }
    
    /// <summary>
    /// Sets temp userdata. 
    /// </summary>
    /// <param name="customer"></param>
    private void SetProperties(Customer customer)
    {
        /*
         * Jag ville vara smart och inte skriva en massa kod för att ange all
         * användardata. Jag kom fram till att man kunde loopa igenom ett objekts
         * properties. I efterhand så känns det lite overkill och ska efter mer
         * reseach typdligen unvikas. Men det funkar :)
         */
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

    /// <summary>
    /// Gets properties from Customer class.
    /// </summary>
    /// <returns></returns>
    private static IEnumerable<PropertyInfo> GetCustomerProperties()
    {
        return typeof(Customer).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    }

    /// <summary>
    /// Excludes properties not to be set by user.
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    private static bool PropertyExclusion(PropertyInfo property)
    {
        return property.PropertyType.IsGenericType &&
               property.PropertyType.GetGenericTypeDefinition() == typeof(List<>) ||
               property.Name is "Funds" or "Username";
    }

    /// <summary>
    /// Edits property name by forcing lower case and adds white space.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private static string FormatPropertyName(string input)
    {
        var splitString = Regex.Replace(input, "([A-Z])", " $1");
        var lowerCaseString = splitString.ToLower();
        return lowerCaseString;
    }

    /// <summary>
    /// Prompts user for current property.
    /// </summary>
    /// <param name="determiner"></param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Returns int if input is a number.
    /// </summary>
    /// <param name="inputString"></param>
    /// <returns></returns>
    private static object ConvertIfInt(object inputString) 
    {
        if (int.TryParse(inputString.ToString(), out var intValue))
        {
            return intValue;
        }

        return inputString;
    }
   
    /// <summary>
    /// Ensure user input is not null.
    /// </summary>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    private static string CheckForInputError(string propertyName)
    {
        const string inputError = "Please actually write something.\n";
        string input;

        while (true)
        {
            Console.WriteLine($"Please enter your {propertyName}.");
            Console.CursorVisible = true;
        
            input = Console.ReadLine();

            if (!string.IsNullOrEmpty(input))
            {
                break;
            }
            
            Console.WriteLine(inputError);
        }

        return input;
    }

    /// <summary>
    /// Returns an or a depending on property name.
    /// </summary>
    /// <param name="word"></param>
    /// <returns></returns>
    private static string GetDeterminer(string word)
    {
        if (string.IsNullOrEmpty(word)) return string.Empty;

        var firstLetter = char.ToLower(word[0]);
        char[] vowels = { 'a', 'e', 'i', 'o', 'u' };

        return Array.IndexOf(vowels, firstLetter) >= 0 ? "an" : "a";
    }

    /// <summary>
    /// Tries to log in user if credentionals are correct.
    /// </summary>
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

    /// <summary>
    /// Logs in user.
    /// </summary>
    /// <param name="customer"></param>
    private void LoginProcess(Customer customer)
    {
        PrintAuthSuccess(customer);
        CurrentCustomer = customer;
        SetState(new AuthenticatedState());
        
        //Ingår i ful-lösningen
        ToggleMenuExeptions();
    }

    /// <summary>
    /// Returns customer if credentionals are correct.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    private Customer CheckCredentials(string username, string password)
    {
        return Customers.FirstOrDefault(c =>
            c.CheckUsername(username) && c.CheckPassword(password));
    }

    /// <summary>
    /// Logs out current user.
    /// </summary>
    public void Logout()
    {
        Console.WriteLine($"\n{CurrentCustomer.Username} logged out\n");
        CurrentCustomer = null;
        SetState(new UnAuthenticatedState());
        ResetTempData();
        
        //Ingår i ful-lösningen
        ToggleMenuExeptions();
        MenuCollection = CreateMenuCollection();
        //Magic number: Index för att uppdatera "Login menu" efter att Register har tagits bort
        MenuCollection.RunMenu(2);
    }

    /// <summary>
    /// Withdraws
    /// </summary>
    /// <param name="customer"></param>
    /// <param name="product"></param>
    public void WithdrawFundsFromCustomer(Customer customer, Product product)
    {
        if (!PurchaseConditions(customer, product))
        {
            return;
        }
        
        customer.Funds -= product.Price;
        customer.AddOrder(product.Name, product.Price);
        product.NrInStock--;

        Console.WriteLine($"Successfully bought {product.Name}\n");
    }

    /// <summary>
    /// Reset temporary credentionals.
    /// </summary>
    private void ResetTempData()
    {
        TempUsername = string.Empty;
        TempPassword = string.Empty;
    }

    /// <summary>
    /// Checks funds of customer and stock of product.
    /// </summary>
    /// <param name="customer"></param>
    /// <param name="product"></param>
    /// <returns></returns>
    private static bool PurchaseConditions(Customer customer, Product product)
    {
        if (!product.ProductIsInStock())
        {
            Console.WriteLine("Not in stock.\n");
            return false;
        }

        if (customer.CanAfford(product.Price)) return true;
        Console.WriteLine("You cannot afford.\n");
        return false;

    }
}