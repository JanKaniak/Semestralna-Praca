using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using SpaceVoyage.Data;
using System.Security.Claims;


namespace SpaceVoyage.Components.Pages.Single_page
{
    public partial class CreateReviewForum
    {

        public DatabaseContext? context;
        public List<Review>? ReviewsList { get; set; }

        [SupplyParameterFromForm]
        public Review? NewReview { get; set; } = new Review();

        public string? ErrorMessage { get; set; }

        public async Task CreateNewReview()
        {
            context ??= await DataContextFactory.CreateDbContextAsync();
            if (context != null && NewReview != null)
            {
                if (!string.IsNullOrWhiteSpace(NewReview.Header) && !string.IsNullOrWhiteSpace(NewReview.Text))
                {
                    if ((NewReview.Rating < 0 || NewReview.Rating > 5))
                    {
                        ErrorMessage = "All fields must be filled in, and a rating must be selected!";
                        return;
                    }
                    var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                    var user = authState.User;
                    if (user != null)
                    {
                        {
                            if (user.Identity.IsAuthenticated)
                            {
                                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                                if (userId != null)
                                {
                                    NewReview.UserId = Int32.Parse(userId);
                                    context.Add(NewReview);
                                    context.SaveChangesAsync();
                                    NavigationManager.NavigateTo("/reviews", true);
                                }


                            }
                        }
                    }


                }
                else
                {
                    ErrorMessage = "All fields must be filled in, and a rating must be selected!";
                    return;
                }
            }
        }
    }
}
