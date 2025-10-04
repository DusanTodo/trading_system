using App;

// Ladda data från fil 
List<User> users = new List<User>();
List<Item> items = new List<Item>();
List<Trade> trades = new List<Trade>();


// Om inga users finns, lägg till en default
if (users.Count() == 0)
{
    users.Add(new User("dt", "pass"));
    users.Add(new User("testuser", "pass"));
}
if (trades.Count() == 0)
{
    trades.Add(new Trade(1, "MSI Gpu 3090 Changes For Asus Gpu 4080", "dt", "testuser"));  //(int itemId, string itemDescription, string senderUsername, string receiverUsername)
}

User? active_user = null;
bool running = true;

while (running)
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Green;

    if (active_user == null)
    {
        Console.Clear();
        Console.WriteLine("Welcome To Trading System In Terminal");
        Console.WriteLine("-------------------------------------");
        Console.WriteLine("1. Log in");
        Console.WriteLine("2. Register");
        Console.WriteLine("3. Exit");
        Console.WriteLine("-------");
        Console.Write("Choose: ");
        string? choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                Console.Clear();
                Console.Write("Username: ");
                string username = Console.ReadLine();
                Console.Write("Password: ");
                string password = Console.ReadLine();
                foreach (User user in users)
                {
                    if (username == user.Username && password == user.Password)
                    {
                        Console.Clear();
                        active_user = user;
                        Console.WriteLine($"Welcome {username}!");
                        Console.ReadLine();
                        break;
                    }
                }
                if (active_user == null)
                {
                    Console.WriteLine("Incorrect username or password, please try again.");
                    Console.ReadLine();
                }
                break;

            case "2":
                Console.Clear();
                Console.Write("Username: ");
                username = Console.ReadLine();
                Console.Clear();
                Console.Write("Password: ");
                password = Console.ReadLine();
                users.Add(new User(username, password));


                Console.Clear();
                Console.WriteLine($"{username} created.");
                Console.ReadLine();
                break;

            case "3":
                running = false;
                break;
        }
    }
    else
    {
        Console.Clear();
        Console.WriteLine($"Logged in as {active_user.Username}");
        Console.WriteLine("-------------------");
        Console.WriteLine("1. Upload item");
        Console.WriteLine("2. Browse available items");
        Console.WriteLine("3. Browse trade requests");
        Console.WriteLine("4. Browse completed requests");
        Console.WriteLine("5. Logout");
        Console.WriteLine("6. Delete account");  //inget krav för detta funktion men någonting som är alltid svårt att hitta på websidor idag:)
        Console.WriteLine("7. Exit");
        Console.WriteLine("-------");
        Console.Write("Choose: ");
        string? choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Console.Clear();
                Console.Write("Item name: ");
                string itemName = Console.ReadLine();
                Console.Write("Description: ");
                string desc = Console.ReadLine();
                items.Add(new Item(itemName, desc, active_user.Username));

               

                Console.WriteLine("Item uploaded!");
                Console.ReadLine();
                break;

            case "2":
                // Browse items (other users' items) + request trade
                Console.Clear();
                Console.WriteLine("Available items (not yours):");
                bool anyItem = false;
                foreach (Item it in items)
                {
                    if (it.Owner != active_user.Username)
                    {
                        anyItem = true;
                        Console.WriteLine($"Id: {it.Id} | {it.Name} | Owner: {it.Owner}");
                        Console.WriteLine($"   {it.Description}");
                    }
                }
                if (!anyItem)
                {
                    Console.WriteLine("-- No items available --");
                    Console.ReadLine();
                    break;
                }

                Console.WriteLine();
                Console.Write("Enter item Id to request trade or press Enter to go back: ");
                string? idInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(idInput))
                    break;

                if (!int.TryParse(idInput, out int itemIdReq))
                {
                    Console.WriteLine("Invalid id.");
                    Console.ReadLine();
                    break;
                }

                // Hitta item
                Item? selItem = null;
                foreach (Item it in items)
                {
                    if (it.Id == itemIdReq) { selItem = it; break; }
                }
                if (selItem == null)
                {
                    Console.WriteLine("Item not found.");
                    Console.ReadLine();
                    break;
                }
                if (selItem.Owner == active_user.Username)
                {
                    Console.WriteLine("You cannot request your own item.");
                    Console.ReadLine();
                    break;
                }

                // Skapa trade (sender = active_user, receiver = selItem.Owner)
                trades.Add(new Trade(selItem.Id, selItem.Description, active_user.Username, selItem.Owner));
                
                Console.WriteLine("Trade request sent.");
                Console.ReadLine();
                break;

            case "3":
                // Browse incoming trades (pending)
                Console.Clear();
                Console.WriteLine("Incoming trade requests (Pending):");
                bool hasIncoming = false;
                foreach (Trade t in trades)
                {
                    if (t.ReceiverUsername == active_user.Username && t.Status == TradeStatus.Pending)
                    {
                        hasIncoming = true;
                        Console.WriteLine($"Trade Id: {t.Id} | ItemId: {t.ItemId} | ItemDescription: {t.ItemDescription} From: {t.SenderUsername}");
                    }
                }
                if (!hasIncoming)
                {
                    Console.WriteLine("-- No incoming requests --");
                    Console.ReadLine();
                    break;
                }

                Console.Write("Enter trade Id to manage or press Enter to go back: ");
                string? tInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(tInput)) break;
                if (!int.TryParse(tInput, out int tradeId))
                {
                    Console.WriteLine("Invalid id.");
                    Console.ReadLine();
                    break;
                }

                // Hitta trade
                Trade? foundTrade = null;
                foreach (Trade t in trades)
                {
                    if (t.Id == tradeId) { foundTrade = t; break; }
                }
                if (foundTrade == null)
                {
                    Console.WriteLine("Trade not found.");
                    Console.ReadLine();
                    break;
                }
                if (foundTrade.ReceiverUsername != active_user.Username)
                {
                    Console.WriteLine("You are not allowed to manage this trade.");
                    Console.ReadLine();
                    break;
                }

                Console.WriteLine("1. Approve");
                Console.WriteLine("2. Deny");
                Console.Write("Choose: ");
                string? act = Console.ReadLine();
                if (act == "1")
                {
                    foundTrade.Approve();
                    // Följande: flytta ägandeskap eller ta bort item.
                    // Här byter vi ägare till avsändaren (för att inte förlora historik)
                    foreach (Item it in items)
                    {
                        if (it.Id == foundTrade.ItemId)
                        {
                            it.Owner = foundTrade.SenderUsername; // OBS: kräver Owner setbar eller byt metod
                            break;
                        }
                    }
                    
                    Console.WriteLine("Trade approved and ownership transferred.");
                    Console.ReadLine();
                }
                else if (act == "2")
                {
                    foundTrade.Deny();
                    
                    Console.WriteLine("Trade denied.");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                    Console.ReadLine();
                }
                break;

            case "4":
                {
                    Console.Clear();
                    Console.WriteLine("Browse Completed Requests:");
                    Console.WriteLine("--------------------------");
                    Console.WriteLine("1. Accepted");
                    Console.WriteLine("2. Denied");
                    Console.WriteLine("3. Go Back");
                    Console.WriteLine("4. Logout");
                    Console.WriteLine("----------");
                    Console.Write("Choose: ");
                    choice = Console.ReadLine();
                    
                    // lokal hjälpmetod för att skriva ut en trade
                    void PrintTrade(Trade t)
                    {
                        string itemName = "(item missing)";
                        foreach (Item it in items)
                        {
                            if (it.Id == t.ItemId)
                            {
                                itemName = it.Name;
                                break;
                            }
                        }
                        Console.WriteLine($"Trade Id: {t.Id} | ItemId: {t.ItemId} ({itemName}) | From: {t.SenderUsername} | To: {t.ReceiverUsername} | Status: {t.Status}");
                    }

                    bool anyFound = false;


                    switch (choice)
                    {
                        case "1":

                            Console.Clear();
                            Console.WriteLine("All completed requests:");
                            foreach (Trade t in trades)
                            {
                                if (t.Status == TradeStatus.Approved)
                                {
                                    anyFound = true;
                                    PrintTrade(t);
                                }
                            }
                            if (!anyFound)
                            Console.WriteLine("NOTE: No approved requests found ");
                            Console.ReadLine();
                            break;

                        case "2":

                            Console.Clear();
                            Console.WriteLine("All Denied Requests");
                            foreach (Trade t in trades)
                            {
                                if (t.Status == TradeStatus.Denied)
                                {
                                    anyFound = true;
                                    PrintTrade(t);
                                }
                            }
                            if (!anyFound)
                            Console.WriteLine("NOTE: No denied requests found ");
                            Console.ReadLine();
                            break;

                        case "3":
                            break;

                        case "4":
                            active_user = null;
                            break;
                    }

                }
                break;


            case "5":
                active_user = null;
                break;

            case "6":
                // Ta bort kontot
                users.Remove(active_user);

                // Ta bort alla items som ägs av användaren
                items.RemoveAll(i => i.Owner == active_user.Username);

                // Ta bort alla trades som är kopplade till användaren
                trades.RemoveAll(t => t.SenderUsername == active_user.Username || t.ReceiverUsername == active_user.Username);

                

                active_user = null;
                Console.WriteLine("Account deleted.");
                Console.ReadLine();
                break;

            case "7":
                running = false;
                break;
        }
    }
}