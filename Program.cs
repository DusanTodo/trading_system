using App;

List<User> users = new list<User>();

users.Add(new User("Dusan", "d@t", "pass"));

User? active_user = null;

bool running = true;

while (running)
{
    Console.Clear();

    Console.WriteLine("Welcome To Trading System In Terminal");
    Console.WriteLine("");

    if (active_user == null)
    {
        Console.Clear()
        Console.WriteLine("1. log in");
        Console.WriteLine("2. register");
        switch(Console.ReadLine());
    }
}