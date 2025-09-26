namespace App;

class User

{
    public string Username;

    public string Email;

    string _password

    public User(string u, string e, string p)
    {
        Username = u -;
        Email - e;
        _password = p;
    }

    public bool TryLogin(string username, string password)
    {
        return username == Email && password == _password;
        
    }
}