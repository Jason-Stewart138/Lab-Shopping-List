using System.Diagnostics.Metrics;
using System.Security.Cryptography.X509Certificates;

Dictionary<string, decimal> itemMenu = new Dictionary<string, decimal>()
{
    {"Milk", 3.29m},
    {"Bread", 2.49m},
    {"Eggs", 2.99m},
    {"Cheese", 4.95m},
    {"Turkey", 7.89m},
    {"Cereal", 4.95m},
    {"Coffee", 14.99m},
    {"Beer", 18.99m},
    {"Apples", 2.99m},
    {"Onions", 1.99m}
};

List<string> userShoppingCartItems = new List<string>();

Console.WriteLine("Welcome To Last Stop Groceries");
Console.WriteLine();
PopulateShoppingCart(out userShoppingCartItems);
ShoppingCartSummary(userShoppingCartItems);
ExitApplication();

void ItemMenu()
{
    Console.WriteLine("Please choose from the following items:");
    Console.WriteLine();
    Console.WriteLine("Item         Price");
    Console.WriteLine("==================");
    int counter = 1;
    foreach (KeyValuePair<string, decimal> item in itemMenu)
    {
        Console.WriteLine($"{counter}. {item.Key,-10} {item.Value.ToString("C"),7}");
        counter++;
    }
    Console.WriteLine();
}

void PopulateShoppingCart(out List<string> userShoppingCartItemsOut)
{
    bool addItem = true;

    while (addItem)
    {
        ItemMenu();
        Console.WriteLine("Please enter the item name or number you would like to purchase:");
        string userSelection = Console.ReadLine().ToLower().Trim();
        Console.WriteLine();
        ValidateUserSelection(userSelection, itemMenu, out List<string> userShoppingCartItems, out bool addItemOut);
        addItem = addItemOut;
    }
    userShoppingCartItemsOut = userShoppingCartItems;
}

void ValidateUserSelection(string userSelection, Dictionary<string, decimal> keyValuePairs, out List<string> userShoppingCartItemsOut, out bool addAnotherItem)
{
    List<string> itemMenuKeys = keyValuePairs.Keys.ToList();
    IEnumerable<string> itemToAdd = itemMenuKeys.Where(x => x.ToLower().Contains(userSelection) && !(userSelection == ""));
    bool isValidKey;
    int userSelectionNumber = -1;
    bool tryAgain = true;

    if (itemToAdd.Count() > 1)
    {
        Console.WriteLine("Your search returned too many results, please try again and narrow your search with more characters.");
        isValidKey = false;
        tryAgain = true;
    }
    else if (itemToAdd.Count() == 1)
    {
        isValidKey = true;
    }
    else
    {
        try
        {
            userSelectionNumber = int.Parse(userSelection);
            if (userSelectionNumber >= itemMenuKeys.Count() + 1 || (userSelectionNumber < 1))
            {
                isValidKey = false;
                tryAgain = false;
            }
            else
            {
                isValidKey = true;
            }
        }
        catch
        {
            isValidKey = false;
            tryAgain = false;
        }
    }

    if (isValidKey)
    {
        string textToDisplay = "";
        if (itemToAdd.Count() > 0)
        {
            foreach (string key in itemToAdd)
            {
                userShoppingCartItems.Add(key);
                textToDisplay = key;
            }
        }
        else
        {
                userShoppingCartItems.Add(itemMenuKeys[userSelectionNumber - 1]);
                textToDisplay = itemMenuKeys[userSelectionNumber - 1];
        }
        if (isValidKey)
        {
            Console.WriteLine($"{textToDisplay} has been added to your cart.");
        }
        
        addAnotherItem = AddAnotherItem("add another item?");
    }
    else if (!tryAgain)
    {
        Console.WriteLine("Sorry, that was an invalid selection.");
        addAnotherItem = AddAnotherItem("add another item?");
    }
    else
    {
        addAnotherItem = AddAnotherItem("add another item?");
    }
    userShoppingCartItemsOut = userShoppingCartItems;
};

void ShoppingCartSummary(List<string> userShoppingCartItems)
{
    List<decimal> itemPrice = new List<decimal>();

    Console.WriteLine("Thank you for your order.");
    Console.WriteLine();
    Console.WriteLine("Your shopping cart summary:");
    Console.WriteLine();
    Console.WriteLine("Item         Price");
    Console.WriteLine("==================");
    foreach (string item in userShoppingCartItems)
    {
        Console.WriteLine($"{item,-10} {itemMenu[item].ToString("C"),7}");
        itemPrice.Add(itemMenu[item]);
    }
    Console.WriteLine();
    Console.WriteLine($"Your total is: {itemPrice.Sum()}");
    Console.WriteLine();
    Console.WriteLine($"The average price per item was: {itemPrice.Average().ToString("C")}");
    Console.WriteLine();
    Console.WriteLine($"The most expensive item you ordered was: {userShoppingCartItems[itemPrice.IndexOf(itemPrice.Max())]} {itemPrice.Max().ToString("C")}");
    Console.WriteLine();
    Console.WriteLine($"The least expensive item you ordered was: {userShoppingCartItems[itemPrice.IndexOf(itemPrice.Min())]} {itemPrice.Min().ToString("C")}");
}

bool AddAnotherItem(string typeOfRepeat)
{
    bool goAgain = false;
    bool isValidInput = true;

    while (isValidInput)
    {
        Console.WriteLine($"Would you like to {typeOfRepeat}(y/n)?");
        string input = Console.ReadLine().ToLower().Trim();
        if (input.Contains("y") || input.Contains("n"))
        {
            goAgain = input == "y" || input == "yes";
            isValidInput = true;
            Console.Clear();
            return goAgain;
        }
        else
        {
            Console.WriteLine("Sorry that was not a valid input, please try again.");
            isValidInput = true;
        }
    }
    return true;
}

void ExitApplication()
{
    Console.WriteLine();
    Console.WriteLine("Thank You For Shopping!");
    Console.WriteLine("Press Any Key To Exit");
    Console.ReadKey();

    Console.Clear();
    Console.WriteLine("Good Bye");
    Environment.Exit(0);
}