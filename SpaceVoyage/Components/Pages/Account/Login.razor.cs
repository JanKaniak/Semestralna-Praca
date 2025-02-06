using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using SpaceVoyage.Data;
using System.Diagnostics;
using System.Security.Claims;

namespace SpaceVoyage.Components.Pages.Account
{
    public partial class Login
    {
        [CascadingParameter]
        public HttpContext? HttpContext { get; set; }
        [SupplyParameterFromForm]
        public LoginInput UserLoginInput { get; set; } = new();
        public string? ErrorMessage { get; set; }
        
        private DatabaseContext? context;
        public string? UserName { get; set; }


        private async Task Authentification()
        {
            context ??= await UserDataContextFactory.CreateDbContextAsync();
            var userAccount = context.Users.FirstOrDefault(x => x.UserName == UserLoginInput.UserName);
            if (string.IsNullOrEmpty(UserLoginInput.UserName) || string.IsNullOrEmpty(UserLoginInput.Password))
            {
                ErrorMessage = "Username and password field can not be empty!";
                return;
            }

            PasswordHasherHandler passwordHasher = new PasswordHasherHandler();
            if (userAccount == null && !passwordHasher.CorrectPassword(userAccount, userAccount.UserPassword, UserLoginInput.Password))
            {
                ErrorMessage = "Invalid username or password";
                return;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, UserLoginInput.UserName),
                new Claim(ClaimTypes.Role, userAccount.UserRole),
                new Claim(ClaimTypes.NameIdentifier, userAccount.UserId.ToString())
            };

            var identity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(principal);
            NavigationManager.NavigateTo("/");
        }
    }
    }
