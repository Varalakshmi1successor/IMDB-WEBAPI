namespace IMDBLite.API.Models.DB;

public class Producer : Person
{
    public Producer()
    {
    }

    public Producer(string name, string bio, DateOnly dateOfBirth, Gender gender)
        : base(name, bio, dateOfBirth, gender)
    {
    }
}