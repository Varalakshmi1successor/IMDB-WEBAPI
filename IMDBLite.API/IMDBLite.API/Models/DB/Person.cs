namespace IMDBLite.API.Models.DB;

public enum Gender
{
    Male,
    Female,
    Others
}

public class Person
{
    public Person()
    {
    }

    public Person(string name, string bio, DateOnly dateOfBirth, Gender gender)
    {
        Name = name;
        Bio = bio;
        DateOfBirth = dateOfBirth;
        Gender = gender;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Bio { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public Gender? Gender { get; set; }
}