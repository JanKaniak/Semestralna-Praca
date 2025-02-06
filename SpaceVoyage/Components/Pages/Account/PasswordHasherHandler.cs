using Microsoft.AspNetCore.Identity;
using SpaceVoyage.Data;

namespace SpaceVoyage.Components.Pages.Account
{
    public class PasswordHasherHandler
    {
        private readonly PasswordHasher<User> passwordHasher;

        public PasswordHasherHandler() { 
            passwordHasher = new PasswordHasher<User>();
        }

        public string PasswordHash(User user,string password)
        {
            return passwordHasher.HashPassword(user, password);
        }

        public bool CorrectPassword(User user, string storedHash, string enteredPassword)
        {
            var result = passwordHasher.VerifyHashedPassword(user, storedHash, enteredPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
