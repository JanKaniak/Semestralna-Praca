using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using SpaceVoyage.Data;
using System.Security.Claims;

namespace SpaceVoyage.Components.Pages.Main
{
    public partial class Reviews
    {
        
        public DatabaseContext? context;
        public List<Review>? ReviewsList { get; set; }

        [SupplyParameterFromForm]
        public Comment? NewReview { get; set; } = new Comment();

        public string? ErrorMessage { get; set; }

        public List<User>? UserList { get; set; }

        public string? UserName { get; set; }
        public bool IsOwner { get; set; }

        private string? UserId { get; set; } 

        protected override async Task OnInitializedAsync()
        {
            context ??= await PatchnoteDataContextFactory.CreateDbContextAsync();
            if (context != null)
            {
                ReviewsList = await context.Reviews.ToListAsync();
                UserList = await context.Users.ToListAsync();
            }

            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user != null) { 
                if (user.Identity.IsAuthenticated)
                {
                    UserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    
                }
            }


        }

        public bool CreatedReview(Review review)
        {
            if (!string.IsNullOrWhiteSpace(UserId))
            {

                if (UserId == review.UserId.ToString())
                {
                    return true;
                }
                if (UserList.Find(x  => x.UserId == Int32.Parse(UserId)).UserRole == "Administrator")
                {
                    return true;
                }
            }
            return false;
        }
    }
}
