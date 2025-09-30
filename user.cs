namespace App;

class User

{
    public string Username;

    public string Email;

    public string Password;

    public User(string u, string e, string p)
    {
        Username = u;
        Email = e;
        Password = p;
    }

    public bool TryLogin(string username, string password)
    {
        return username == Email && password == Password;

    }
}