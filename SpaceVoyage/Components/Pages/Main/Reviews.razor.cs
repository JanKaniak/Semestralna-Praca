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

        protected override async Task OnInitializedAsync()
        {
            context ??= await PatchnoteDataContextFactory.CreateDbContextAsync();
            if (context != null)
            {
                ReviewsList = await context.Reviews.ToListAsync();
                UserList = await context.Users.ToListAsync();
            }

        }

        public async Task CreateNewReview()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;


            if (user.Identity.IsAuthenticated)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId != null)
                {
                    NewReview.UserId = Int32.Parse(userId);
                    context?.Comments?.Add(NewReview);
                    context?.SaveChangesAsync();
                    await JS.InvokeVoidAsync("eval", "window.location.reload();");

                }
            }

        }

        public async Task CreatedReview(Review review)
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;


            if (user.Identity.IsAuthenticated)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    if (userId == review.UserId.ToString())
                    {
                        IsOwner = true;
                    } else
                    {
                        IsOwner = false;
                    }
                }

                
            }
        }
    }
}
