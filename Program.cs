using App;

// --- Ladda data från fil ---
List<User> users;
List<Item> items;
List<Trade> trades;
Database.Load(out users, out items, out trades);

// Om inga users finns, lägg till en default
if (users.Count == 0)
{
    users.Add(new User("d@t", "pass"));
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

                // --- Spara direkt efter ändring ---
                Database.Save(users, items, trades);

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
        Console.WriteLine("2. Browse items");
        Console.WriteLine("3. Browse trade requests");
        Console.WriteLine("4. Logout");
        Console.WriteLine("5. Delete account");
        Console.WriteLine("6. Exit");
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

                // --- Spara direkt efter ändring ---
                Database.Save(users, items, trades);

                Console.WriteLine("Item uploaded!");
                Console.ReadLine();
                break;

            case "2":
                // Browse items (other users' items) + request trade
                Console.Clear();
                Console.WriteLine("Available items (not yours):");
                bool anyItem = false;
                foreach (var it in items)
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
                foreach (var it in items)
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
                trades.Add(new Trade(selItem.Id, active_user.Username, selItem.Owner));
                Database.Save(users, items, trades); // spara direkt
                Console.WriteLine("Trade request sent.");
                Console.ReadLine();
                break;

            

            case "4":
                active_user = null;
                break;

            case "5":
                // Ta bort kontot
                users.Remove(active_user);

                // Ta bort alla items som ägs av användaren
                items.RemoveAll(i => i.Owner == active_user.Username);

                // Ta bort alla trades som är kopplade till användaren
                trades.RemoveAll(t => t.SenderUsername == active_user.Username || t.ReceiverUsername == active_user.Username);

                // --- Spara direkt efter ändring ---
                Database.Save(users, items, trades);

                active_user = null;
                Console.WriteLine("Account deleted.");
                Console.ReadLine();
                break;

            case "6":
                running = false;
                break;
        }
    }
}