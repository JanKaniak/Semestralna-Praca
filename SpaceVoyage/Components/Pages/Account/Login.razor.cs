using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using SpaceVoyage.Data;
using SpaceVoyage.Models.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

namespace SpaceVoyage.Components.Pages.Account
{
    public partial class Login
    {
        [CascadingParameter]
        public HttpContext? HttpContext { get; set; }
        [SupplyParameterFromForm]
        public LoginViewModel ViewModel { get; set; } = new();
        public string? ErrorMessage { get; set; }
        
        private UserDataContext? context;
        public string? UserName { get; set; }


        private async Task Authentification()
        {
            context ??= await UserDataContextFactory.CreateDbContextAsync();
            var userAccount = context.Users.FirstOrDefault(x => x.UserName == ViewModel.UserName);
            /*if (string.IsNullOrWhiteSpace(ViewModel.UserName) || string.IsNullOrWhiteSpace(ViewModel.Password))
            {
                ErrorMessage = "Invalid user name or password! (ㆆ _ ㆆ)";
                return;
            }*/
  
            if (userAccount == null || userAccount.UserPassword != ViewModel.Password)
            {
                ErrorMessage = $"Invalid password, correct password for user {ViewModel.UserName} is: {userAccount.UserPassword}";
                return;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, ViewModel.UserName),
                new Claim(ClaimTypes.Role, userAccount.UserRole)
            };

            var identity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(principal);
            NavigationManager.NavigateTo("/");
        }
    }
    }
