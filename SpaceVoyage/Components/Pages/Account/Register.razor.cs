using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using SpaceVoyage.Data;
using System.Diagnostics;
using System.Security.Claims;
using SpaceVoyage.Components.Pages.Account;

namespace SpaceVoyage.Components.Pages.Account
{
    public partial class Register
    {
        [CascadingParameter]
        public HttpContext? HttpContext { get; set; }

        [SupplyParameterFromForm]
        public RegisterInput UserRegisterInput { get; set; } = new();
        public string? ErrorMessage { get; set; }

        private DatabaseContext? context;

        private PasswordHasherHandler passwordHasherHandler;


        private async Task Registration()
        {
            context ??= await DataContextFactory.CreateDbContextAsync();
            if (context != null)
            {
                var user = context.Users.FirstOrDefault(x => x.UserName == UserRegisterInput.UserName);
                if (user != null)
                {
                    ErrorMessage = "Username already exists!";
                    return;
                }

                user = null;
                user = context.Users.FirstOrDefault(x => x.Name == UserRegisterInput.Name);
                if (user != null)
                {
                    ErrorMessage = "Name already exists!";
                    return;
                }
                if (string.IsNullOrWhiteSpace(UserRegisterInput.Password) && string.IsNullOrWhiteSpace(UserRegisterInput.UserName) && string.IsNullOrWhiteSpace(UserRegisterInput.Name))
                {
                    ErrorMessage = "All fields need to be filled in!";
                    return;
                }

                user = new User();
                user.UserName = UserRegisterInput.UserName;
                user.Name = UserRegisterInput.Name;
                user.UserRole = "User";
                passwordHasherHandler = new PasswordHasherHandler();
                user.UserPassword = passwordHasherHandler.PasswordHash(user,UserRegisterInput.Password);
                context.Users.AddAsync(user);
                context.SaveChangesAsync();

                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.UserRole),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(principal);
                NavigationManager.NavigateTo("/");


            }

            

            
        }
    }
}
