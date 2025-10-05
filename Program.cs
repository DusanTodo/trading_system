
using App;

List<User> users = new List<User>(); //varje rad skaffar toma listor för att programet ska kunna spara data tillfälligt i minne
List<Item> items = new List<Item>();
List<Trade> trades = new List<Trade>();

SaveLoad.LoadAll(users, items, trades); //funktion från saveload.cs klassen, gör att sprade data i filer kan laddas in i programet

if (users.Count() == 0) //lägger in default users om inga registrerade finns
{
    users.Add(new User("dt", "pass"));
    users.Add(new User("testuser", "pass"));
    SaveLoad.LoadAll(users, items, trades);
}
if (trades.Count() == 0) //lägger in default trades om inga registrerade finns
{
    trades.Add(new Trade(1, "MSI Gpu 3090 Changes For Asus Gpu 4080", "dt", "testuser"));  //(int itemId, string itemDescription, string senderUsername, string receiverUsername)
    SaveLoad.LoadAll(users, items, trades);
}

User? active_user = null; //variabeln håller koll på vilket användare är inloggat, startar som null inga inloggade
bool running = true; //styr om huvudloopen och håller programmet att fortsätta köras, eller avslutas(false)

while (running) //while loopen, så länge running is true så visas det menuy skärm, med valbara funktioner
{
    Console.Clear(); //rensar skärmen
    Console.ForegroundColor = ConsoleColor.Green; //bestämmer font färg

    if (active_user == null) //active user null inga inloggade så visas inloggnings meny
    {
        Console.Clear();
        Console.WriteLine("Welcome To Trading System In Terminal");
        Console.WriteLine("-------------------------------------");
        Console.WriteLine("1. Log in");
        Console.WriteLine("2. Register");
        Console.WriteLine("3. Exit");
        Console.WriteLine("-------");
        Console.Write("Choose: ");
        string? choice = Console.ReadLine(); //läser in tal val(1,2,3)
        switch (choice) //gör ett val beroende vad användaren väljer
        {
            case "1":
                Console.Clear();
                Console.Write("Username: ");
                string username = Console.ReadLine();
                Console.Write("Password: ");
                string password = Console.ReadLine();
                foreach (User user in users) //kollar alla användare/users i listan för matchning
                {
                    if (username == user.Username && password == user.Password)
                    {
                        Console.Clear();
                        active_user = user; //logga in användaren
                        Console.WriteLine($"Welcome {username}!");
                        Console.ReadLine();
                        break;
                    }
                }
                if (active_user == null) //om username/lösen matchar inte skrivs det felmeddelande
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
                users.Add(new User(username, password)); //nytt user läggs till i listan
                SaveLoad.SaveAll(users, items, trades); //nytt user sparas i filen


                Console.Clear();
                Console.WriteLine($"{username} created.");
                Console.ReadLine();
                break;

            case "3":
                running = false; //aslutar programet
                break;
        }
    }
    else //meny för inloggad användare/user
    {
        Console.Clear();
        Console.WriteLine($"Logged in as {active_user.Username}");
        Console.WriteLine("-------------------");
        Console.WriteLine("1. Upload item");
        Console.WriteLine("2. Browse available items");
        Console.WriteLine("3. Browse trade requests");
        Console.WriteLine("4. Browse completed requests");
        Console.WriteLine("5. Logout");
        Console.WriteLine("6. Delete account"); //inget krav för detta funktion men någonting som är alltid svårt att hitta på websidor idag:)
        Console.WriteLine("7. Exit");
        Console.WriteLine("-------");
        Console.Write("Choose: ");
        string? choice = Console.ReadLine();

        switch (choice)
        {
            case "1": //ladda upp nytt item/objekt, id tilldelas automatisk, usern anger namn och beskrivning
                Console.Clear();
                Console.Write("Item name: ");
                string itemName = Console.ReadLine();
                Console.Write("Description: ");
                string desc = Console.ReadLine();
                items.Add(new Item(itemName, desc, active_user.Username));
                SaveLoad.SaveAll(users, items, trades);

               

                Console.WriteLine("Item uploaded!");
                Console.ReadLine();
                break;

            case "2": //visar tillgängliga items från andra användare
                Console.Clear();
                Console.WriteLine("Available items (not yours):");
                bool anyItem = false;
                foreach (Item it in items)
                {
                    if (it.Owner != active_user.Username) //utesluter objekt/items från den inloggade användare/user
                    {
                        anyItem = true;
                        Console.WriteLine($"Id: {it.Id} | {it.Name} | Owner: {it.Owner}");
                        Console.WriteLine($"   {it.Description}");
                    }
                }
                if (!anyItem) //om inga items finns i listan
                {
                    Console.WriteLine("-- No items available --");
                    Console.ReadLine();
                    break;
                }

                Console.WriteLine();
                Console.Write("Enter item Id to request trade or press Enter to go back: ");
                string? idInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(idInput)) //om inget skrivs, hoppar menyn tillbaka till föregående meny
                    break;

                if (!int.TryParse(idInput, out int itemIdReq)) //om ID finns inte, visas meddelande invalid ID
                {
                    Console.WriteLine("Invalid id.");
                    Console.ReadLine();
                    break;
                }

                Item? selItem = null;
                foreach (Item it in items) //går igenom object i listan items
                {
                    if (it.Id == itemIdReq) { selItem = it; break; } //om objektets ID och den id som usern har skrivit matchar då avslutas loopen
                }
                if (selItem == null) //om ID matchar inte
                {
                    Console.WriteLine("Item not found.");
                    Console.ReadLine();
                    break;
                }
                if (selItem.Owner == active_user.Username) //om objektet är usern eget objekt, då kan inte utbyte förfrågas
                {
                    Console.WriteLine("You cannot request your own item.");
                    Console.ReadLine();
                    break;
                }

                trades.Add(new Trade(selItem.Id, selItem.Description, active_user.Username, selItem.Owner)); //skapa trade förfrågan mellan ägare och inloggat user
                SaveLoad.SaveAll(users, items, trades);
                
                Console.WriteLine("Trade request sent.");
                Console.ReadLine();
                break;

            case "3":
                Console.Clear();
                Console.WriteLine("Incoming trade requests (Pending):");
                bool hasIncoming = false;
                foreach (Trade t in trades)
                {
                    if (t.ReceiverUsername == active_user.Username && t.Status == TradeStatus.Pending) //visa inkommande trade förfrågningar med status pending
                    {
                        hasIncoming = true;
                        Console.WriteLine($"Trade Id: {t.Id} | ItemId: {t.ItemId} | ItemDescription: {t.ItemDescription} From: {t.SenderUsername}");
                    }
                }
                if (!hasIncoming) //om inga förfrågningar finns
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

                Trade? foundTrade = null; //hitta trade
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
                if (act == "1") //val av åtgärd approve/deny, funderade att använda choice som innan, men det blev act
                {
                    foundTrade.Approve();
                    
                    foreach (Item it in items)
                    {
                        if (it.Id == foundTrade.ItemId)
                        {
                            it.Owner = foundTrade.SenderUsername; //byter items ägandeskap
                            break;
                        }
                    }
                    SaveLoad.SaveAll(users, items, trades);
                    
                    Console.WriteLine("Trade approved and ownership transferred.");
                    Console.ReadLine();
                }
                else if (act == "2")
                {
                    foundTrade.Deny();
                    SaveLoad.SaveAll(users, items, trades);
                    
                    Console.WriteLine("Trade denied.");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                    Console.ReadLine();
                }
                break;

            case "4": //visar historik av trade förfrågningar
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
                    
                    void PrintTrade(Trade t) //skriver ut en trade med id och egenskaper
                    {
                        string itemName = "(item missing)";
                        foreach (Item it in items) //
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
                        case "1": //visaralla godkända trade förfrågningar

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

            case "6": //funktion att radera konto, var inget krav men tänkte att ja måste implementera den pga att den saknas eller är svårt hittad idag på nätet:)
                users.Remove(active_user);

                items.RemoveAll(i => i.Owner == active_user.Username); 

                trades.RemoveAll(t => t.SenderUsername == active_user.Username || t.ReceiverUsername == active_user.Username);
                SaveLoad.SaveAll(users, items, trades);

                

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