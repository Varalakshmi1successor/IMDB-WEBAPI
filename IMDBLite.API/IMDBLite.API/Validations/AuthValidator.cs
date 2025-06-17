using IMDBLite.API.Models.DB;
using IMDBLite.API.Models.DTOs.Request;
using IMDBLite.API.Validations.Interfaces;
using static IMDBLite.API.Exceptions.CustomExceptions;

public class AuthValidator : IAuthValidator
{
    public void ValidateSignup(SignupRequest request, User? existingUser)
    {
        if (existingUser != null)
            throw new InvalidAuthException("User already exists");
    }

    public void ValidateLogin(LoginRequest request, User? user)
    {
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            throw new InvalidAuthException("Invalid email or password");
    }

    public void ValidateUpdate(SignupRequest request, User givenUser, int id)
    {
        if (givenUser == null)
            throw new InvalidAuthException("User not found");
    }

    public void ValidateExists(User? user, int id)
    {
        if (user == null)
            throw new InvalidAuthException($"User with ID {id} does not exist");
    }

    public void ValidateDuplicates(SignupRequest request, IEnumerable<User> existingUsers, int id)
    {
        var duplicateEmail = existingUsers.FirstOrDefault(u => u.Email == request.Email && u.Id != id);
        if (duplicateEmail != null)
            throw new InvalidAuthException("Email is already taken by another user");
    }
}