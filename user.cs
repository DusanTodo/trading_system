namespace App;

public class User //klassen User representerar en användare i systemet

{
    public string Username;

    public string Password;

    public User(string u, string p) //konstruktor, används när vi registrerar new user
    {
        Username = u;
        Password = p;
    }

    public bool TryLogin(string username, string password) //enkelt metod att försöka logga in 
    {
        return username == Username;

    }
}