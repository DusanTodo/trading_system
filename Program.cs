using App;

List<User> users = new List<User>();

users.Add(new User("d@t", "pass"));

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
                System.Console.Write("Username: ");
                string username = Console.ReadLine();
                System.Console.Write("Password: ");
                string password = Console.ReadLine();
                foreach (User user in users)
                {
                    if (username == user.Username && password == user.Password)
                    {
                        Console.Clear();
                        active_user = user;
                        System.Console.WriteLine($"Welcome {username}!");
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
                System.Console.Write("Username: ");
                username = Console.ReadLine();
                Console.Clear();
                System.Console.Write("Password: ");
                password = Console.ReadLine();
                users.Add(new User(username, password));
                Console.Clear();
                System.Console.WriteLine($"{username} created.");
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
            



            case "4":
                active_user = null;
                break;


        }
    }
    
}