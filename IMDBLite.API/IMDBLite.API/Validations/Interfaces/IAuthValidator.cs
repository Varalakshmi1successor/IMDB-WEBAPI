using IMDBLite.API.Models.DB;
using IMDBLite.API.Models.DTOs.Request;

namespace IMDBLite.API.Validations.Interfaces;

public interface IAuthValidator
{
    void ValidateSignup(SignupRequest request, User? existingUser);
    void ValidateLogin(LoginRequest request, User? user);
    void ValidateUpdate(SignupRequest request, User givenUser, int id);
    void ValidateExists(User? user, int id);
    void ValidateDuplicates(SignupRequest request, IEnumerable<User> existingUsers, int id);
}