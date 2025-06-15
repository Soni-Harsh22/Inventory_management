using InventoryManagementSystem.BussinessService.Interface;
using InventoryManagementSystem.Common.Enums;
using InventoryManagementSystem.Common.Helper;
using InventoryManagementSystem.DataService.Interface;
using InventoryManagementSystem.Models.Entity;
using InventoryManagementSystem.Models.Request;
using InventoryManagementSystem.Models.Responces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Serilog;
using InventoryManagementSystem.Common.Constants;

namespace InventoryManagementSystem.BussinessService.Implemantations;

public class UserService : IUserService
{
    private readonly IUserDataService _userDataService;
    private readonly IConfiguration _config;

    public UserService(IUserDataService userDataService, IConfiguration config)
    {
        _userDataService = userDataService;
        _config = config;
    }
    /// <summary>
    /// Registers a new user in the system.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<ApiResponse<string>> RegisterAsync(UserRegisterRequest request)
    {
        try
        {
            var isValidRole = await _userDataService.IsValidRoleIdAsync(request.RoleId);
            if (!isValidRole)
            {
                Log.Warning("Attempt to register with invalid role ID: {RoleId}", request.RoleId);
                return ApiResponse<string>.CreateResponse(ResponseCode.BadRequest, UserConstants.INVALID_ROLE_ID);
            }

            if (string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Phone) || string.IsNullOrEmpty(request.Password))
            {
                Log.Warning("Attempt to register with missing required fields");
                return ApiResponse<string>.CreateResponse(ResponseCode.BadRequest, UserConstants.ALL_FIELDS_REQUIRED);
            }

            if (request.Password.Length < 6)
            {
                Log.Warning("Attempt to register with weak password");
                return ApiResponse<string>.CreateResponse(ResponseCode.BadRequest, UserConstants.PASSWORD_TOO_SHORT);
            }
            var existingUser = await _userDataService.GetUserByEmailAsync(request.Email);
            if (existingUser != null)
            {
                Log.Warning("Attempt to register with existing email: {Email}", request.Email);
                return ApiResponse<string>.CreateResponse(ResponseCode.Conflict, UserConstants.EMAIL_ALREADY_EXISTS);
            }

            var newUser = new Users
            {
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                RoleId = request.RoleId,
                CreatedBy = (int)Role.System,
                CreatedDate = DateTime.Now,
                Password = ""
            };

            newUser.Password = PasswordHashing.HashPassword(newUser, request.Password ?? string.Empty);

            await _userDataService.AddUserAsync(newUser);

            Log.Information("New user registered: {Email}", newUser.Email);
            return ApiResponse<string>.CreateResponse(ResponseCode.Created, UserConstants.USER_REGISTERED_SUCCESSFULLY);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred during user registration.");
            return ApiResponse<string>.CreateResponse(ResponseCode.InternalServerError, UserConstants.SOMETHING_WENT_WRONG);
        }
    }
    /// <summary>
    /// Handles user login by validating credentials and generating a JWT token.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<ApiResponse<UserLoginResponce>> LoginAsync(UserLoginRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                Log.Warning("Login failed: Missing email or password");
                return ApiResponse<UserLoginResponce>.CreateResponse(ResponseCode.BadRequest, UserConstants.EMAIL_PASSWORD_REQUIRED);
            }

            var existingUser = await _userDataService.GetUserByEmailAsync(request.Email);

            if (existingUser == null || string.IsNullOrEmpty(existingUser.Password))
            {
                Log.Warning("Login failed: Invalid credentials for {Email} Email already exists", request.Email);
                return ApiResponse<UserLoginResponce>.CreateResponse(ResponseCode.Unauthorized, UserConstants.INVALID_CREDENTIALS);
            }

            bool isPasswordValid = PasswordHashing.VerifyPassword(existingUser, existingUser.Password, request.Password);
            if (!isPasswordValid)
            {
                Log.Warning("Login failed: Invalid password for {Email}", request.Email);
                return ApiResponse<UserLoginResponce>.CreateResponse(ResponseCode.Unauthorized, UserConstants.INVALID_CREDENTIALS);
            }

            var accessToken = GenerateJwtToken(existingUser, 15); // 15 min
            var refreshToken = GenerateJwtToken(existingUser, 10080); // 7 days (60 * 24 * 7)

            var response = new UserLoginResponce
            {
                UserId = existingUser.UserId,
                Name = existingUser.Name,
                RoleId = existingUser.RoleId,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            Log.Information("User logged in successfully: {Email}", request.Email);
            return new ApiResponse<UserLoginResponce>(ResponseCode.Ok, UserConstants.LOGIN_SUCCESSFUL, response) { Token = accessToken };
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred during login for {Email}", request.Email);
            return ApiResponse<UserLoginResponce>.CreateResponse(ResponseCode.InternalServerError, UserConstants.SOMETHING_WENT_WRONG);
        }
    }
    /// <summary>
    /// Generates a JWT token for the user based on their claims.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    private string GenerateJwtToken(Users user)
    {
        var claims = new[]
        {
            new Claim("UserId", user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email ?? ""),
            new Claim(ClaimTypes.Role, user.Role?.Name ?? "")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    private string GenerateJwtToken(Users user, int expireMinutes)
    {
        var claims = new[]
        {
                    new Claim("UserId", user.UserId.ToString()),
                    new Claim(ClaimTypes.Email, user.Email ?? ""),
                    new Claim(ClaimTypes.Role, user.Role?.Name ?? "")
                };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expireMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<ApiResponse<UserLoginResponce>> RefreshTokenAsync(string refreshToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);

        try
        {
            var principal = tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidAudience = _config["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            }, out SecurityToken validatedToken);

            long userId = long.TryParse(principal.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value, out var parsedUserId) ? parsedUserId : 0;

            if (userId == 0)
                return ApiResponse<UserLoginResponce>.CreateResponse(ResponseCode.Unauthorized, "Invalid refresh token");

            // Fetch the user entity using the userId
            var user = await _userDataService.GetUserByIdAsync(userId);
            if (user is null)
                return ApiResponse<UserLoginResponce>.CreateResponse(ResponseCode.Unauthorized, "User not found");

            var newAccessToken = GenerateJwtToken(user, 1);
            var newRefreshToken = GenerateJwtToken(user, 10080);

            var response = new UserLoginResponce
            {
                UserId = user.UserId,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };

            return ApiResponse<UserLoginResponce>.CreateResponse(ResponseCode.Ok, "Token refreshed successfully", response);
        }
        catch
        {
            return ApiResponse<UserLoginResponce>.CreateResponse(ResponseCode.Unauthorized, "Invalid refresh token");
        }
    }
}