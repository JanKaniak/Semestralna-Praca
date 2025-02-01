using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SpaceVoyage.Data;
using System.Security.Claims;

namespace SpaceVoyage.Components.Pages.Single_page
{
    public partial class ForumPostSite
    {
        [Parameter]
        public string Id { get; set; }
        public DatabaseContext? context;
        public List<Post>? PostsList { get; set; }

        public Comment? NewComment { get; set; }


        public Post? PostToShow { get; set; }

        public string? ErrorMessage  { get; set; }

        protected override async Task OnInitializedAsync()
        {
            context ??= await PatchnoteDataContextFactory.CreateDbContextAsync();
            if (context != null)
            {
                PostToShow = context.Posts.FirstOrDefault(x => x.Id == Int32.Parse(Id));
                PostsList = await context.Posts.ToListAsync();
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

                }
            }

        }
    }
}
