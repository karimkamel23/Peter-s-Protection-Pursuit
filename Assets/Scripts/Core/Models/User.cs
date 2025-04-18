using System;

[Serializable]
public class User
{
    public int id;
    public string username;
    public string email;

    // Constructor for creating a new user instance
    public User(int id, string username, string email)
    {
        this.id = id;
        this.username = username;
        this.email = email;
    }
} 