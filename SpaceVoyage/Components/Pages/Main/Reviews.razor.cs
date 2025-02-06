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
        public Comment? NewComment { get; set; } = new Comment();

        public string? ErrorMessage { get; set; }

        public List<Comment>? CommentList { get; set; }

        public List<User>? UserList { get; set; }

        protected override async Task OnInitializedAsync()
        {
            context ??= await PatchnoteDataContextFactory.CreateDbContextAsync();
            if (context != null)
            {
                //ReviewsList = await context.Posts.ToListAsync();
                var comments = await context.Comments.ToListAsync();
                UserList = await context.Users.ToListAsync();
            }

        }

        public async Task CreateNewComment()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;


            if (user.Identity.IsAuthenticated)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId != null)
                {
                    NewComment.UserId = Int32.Parse(userId);
                    context?.Comments?.Add(NewComment);
                    context?.SaveChangesAsync();
                    await JS.InvokeVoidAsync("eval", "window.location.reload();");

                }
            }

        }
    }
}
