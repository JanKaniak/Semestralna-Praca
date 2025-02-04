using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
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

        [SupplyParameterFromForm]
        public Comment? NewComment { get; set; } = new Comment();


        public Post? PostToShow { get; set; }

        public string? ErrorMessage  { get; set; }

        public List<Comment>? CommentList { get; set; }

        public List<User>? UserList { get; set; }

        protected override async Task OnInitializedAsync()
        {
            context ??= await PatchnoteDataContextFactory.CreateDbContextAsync();
            if (context != null)
            {
                PostToShow = context.Posts.FirstOrDefault(x => x.Id == Int32.Parse(Id));
                PostsList = await context.Posts.ToListAsync();
                var comments = await context.Comments.ToListAsync();
                CommentList = comments.FindAll(x =>  x.PostId == Int32.Parse(Id));
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
                    NewComment.PostId = Int32.Parse(Id);
                    context?.Comments?.Add(NewComment);
                    context?.SaveChangesAsync();
                    await JS.InvokeVoidAsync("eval", "window.location.reload();");

                }
            }

        }
    }
}
