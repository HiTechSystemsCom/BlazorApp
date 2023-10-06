using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using BlazorApp.Data;
using BlazorApp.Data.Entities;
using BlazorApp.Utils;

// MODIF ADDED BY RH 04.10.2023
using BlazorApp.Data.WebAPI;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace BlazorApp.Services;
public class UserService : IUserService
{
    public HttpClient _httpClient { get; }
    public AppSettings _appSettings { get; }

    public UserService(HttpClient httpClient, IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings.Value;

        //httpClient.BaseAddress = new Uri(_appSettings.BookStoresBaseAddress);
        httpClient.BaseAddress = new Uri("https://localhost:7041");
        httpClient.DefaultRequestHeaders.Add("User-Agent", "BlazorServer");

        _httpClient = httpClient;
    }

    public async Task<User> LoginAsync(User user)
    {
        user.Password = EncryptionUtils.Encrypt(user.Password);
        string serializedUser = JsonConvert.SerializeObject(user);

        //var requestMessage = new HttpRequestMessage(HttpMethod.Post, "Users/Login");
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://localhost:54480/Users/Login");    // MODIF BY RH 30.05.2023
        requestMessage.Content = new StringContent(serializedUser);

        requestMessage.Content.Headers.ContentType
            = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

        var response = await _httpClient.SendAsync(requestMessage);

        var responseStatusCode = response.StatusCode;
        var responseBody = await response.Content.ReadAsStringAsync();

        var returnedUser = JsonConvert.DeserializeObject<User>(responseBody);

        return await Task.FromResult(returnedUser);

    }

    // FROM WEB API
    public async Task<UserWithToken> WebAPI_Login(User user)
    {
        //user = await _context.Users.Include(u => u.Role)
        //                            .Where(u => u.EmailAddress == user.EmailAddress
        //                                    && u.Password == user.Password).FirstOrDefaultAsync();

        UserWithToken userWithToken = null;

        if (user != null)
        {
            RefreshToken refreshToken = GenerateRefreshToken();
            //user.RefreshTokens.Add(refreshToken);
            //await _context.SaveChangesAsync();                          // Save token to DB via EF Core

            user.RefreshToken = refreshToken.Token;   // MODIF ADDED BY RH 04.10.2023

            userWithToken = new UserWithToken(user);
            userWithToken.RefreshToken = refreshToken.Token;
        }

        //if (userWithToken == null)
        //{
        //    return NotFound();
        //}

        //sign your token here here..
        userWithToken.AccessToken = GenerateAccessToken(user.UserId);
        return userWithToken;                                           // Return response
    }

    private RefreshToken GenerateRefreshToken()
    {
        RefreshToken refreshToken = new RefreshToken();

        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            refreshToken.Token = Convert.ToBase64String(randomNumber);
        }
        refreshToken.ExpiryDate = DateTime.UtcNow.AddMonths(6);

        return refreshToken;
    }

    private string GenerateAccessToken(int userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        //var key = Encoding.ASCII.GetBytes(_jwtsettings.SecretKey);
        var key = Encoding.ASCII.GetBytes("ABCDEFGH");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                    new Claim(ClaimTypes.Name, Convert.ToString(userId))
            }),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }


    public async Task<User> RegisterUserAsync(User user)
    {
        user.Password = EncryptionUtils.Encrypt(user.Password);
        string serializedUser = JsonConvert.SerializeObject(user);

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "Users/RegisterUser");
        requestMessage.Content = new StringContent(serializedUser);

        requestMessage.Content.Headers.ContentType
            = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

        var response = await _httpClient.SendAsync(requestMessage);

        var responseStatusCode = response.StatusCode;
        var responseBody = await response.Content.ReadAsStringAsync();

        var returnedUser = JsonConvert.DeserializeObject<User>(responseBody);

        return await Task.FromResult(returnedUser);
    }

    public async Task<User> RefreshTokenAsync(RefreshRequest refreshRequest)
    {
        string serializedUser = JsonConvert.SerializeObject(refreshRequest);

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "Users/RefreshToken");
        requestMessage.Content = new StringContent(serializedUser);

        requestMessage.Content.Headers.ContentType
            = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

        var response = await _httpClient.SendAsync(requestMessage);

        var responseStatusCode = response.StatusCode;
        var responseBody = await response.Content.ReadAsStringAsync();

        var returnedUser = JsonConvert.DeserializeObject<User>(responseBody);

        return await Task.FromResult(returnedUser);
    }

    public async Task<User> GetUserByAccessTokenAsync(string accessToken)
    {
        string serializedRefreshRequest = JsonConvert.SerializeObject(accessToken);

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "Users/GetUserByAccessToken");
        requestMessage.Content = new StringContent(serializedRefreshRequest);

        requestMessage.Content.Headers.ContentType
            = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

        var response = await _httpClient.SendAsync(requestMessage);

        var responseStatusCode = response.StatusCode;
        var responseBody = await response.Content.ReadAsStringAsync();

        var returnedUser = JsonConvert.DeserializeObject<User>(responseBody);

        return await Task.FromResult(returnedUser);
    }
}
