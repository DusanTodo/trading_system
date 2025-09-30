namespace App;

class User

{
    public string Username;

    public string Password;

    public User(string u, string p)
    {
        Username = u;
        Password = p;
    }

    public bool TryLogin(string username, string password)
    {
        return username == Password;

    }
}