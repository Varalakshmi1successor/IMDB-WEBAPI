using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using IMDBLite.API.Models.DB;
using IMDBLite.API.Models.DTOs.Request;
using IMDBLite.API.Models.DTOs.Response;
using IMDBLite.API.Repository.Interfaces;
using IMDBLite.API.Services.Interfaces;
using IMDBLite.API.Validations.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace IMDBLite.API.Services;

public class AuthService : IAuthService
{
    private readonly IAuthValidator _authValidator;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository, IConfiguration configuration, IMapper mapper,
        IAuthValidator authValidator)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _mapper = mapper;
        _authValidator = authValidator;
    }

    public async Task<MessageResponse> SignupAsync(SignupRequest request)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        _authValidator.ValidateSignup(request, existingUser);

        var user = _mapper.Map<User>(request);
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

        await _userRepository.AddAsync(user);

        return new MessageResponse
        {
            Id = user.Id,
            Message = "User signed up successfully."
        };
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        _authValidator.ValidateLogin(request, user);

        var token = GenerateJwtToken(user);
        var response = _mapper.Map<LoginResponse>(user);
        response.Token = token;

        return response;
    }

    public async Task<List<SignupResponse>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return _mapper.Map<List<SignupResponse>>(users);
    }

    public async Task<SignupResponse> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        _authValidator.ValidateExists(user, id);
        return _mapper.Map<SignupResponse>(user);
    }

    public async Task<MessageResponse> UpdateAsync(int id, SignupRequest request)
    {
        var givenUser = await _userRepository.GetByIdAsync(id);
        var existingUsers = await _userRepository.GetAllAsync();

        _authValidator.ValidateUpdate(request, givenUser, id);
        _authValidator.ValidateDuplicates(request, existingUsers, id);

        var user = _mapper.Map<User>(request);
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

        await _userRepository.UpdateAsync(id, user);

        return new MessageResponse
        {
            Id = id,
            Message = "User updated successfully."
        };
    }

    public async Task<MessageResponse> DeleteAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        _authValidator.ValidateExists(user, id);
        await _userRepository.DeleteAsync(id);

        return new MessageResponse
        {
            Id = id,
            Message = "User deleted successfully."
        };
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Email),
            new Claim("FirstName", user.FirstName),
            new Claim("LastName", user.LastName)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _configuration["JWT:ValidIssuer"],
            _configuration["JWT:ValidAudience"],
            claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}