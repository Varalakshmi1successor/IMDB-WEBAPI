namespace IMDBLite.API.Models.DB;

public class Actor : Person
{
    public Actor()
    {
    }

    public Actor(string name, string bio, DateOnly dateOfBirth, Gender gender)
        : base(name, bio, dateOfBirth, gender)
    {
    }
}