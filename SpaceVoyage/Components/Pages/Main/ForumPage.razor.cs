using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using SpaceVoyage.Data;

namespace SpaceVoyage.Components.Pages.Main
{
    public partial class ForumPage
    {
        [Parameter]
        public string pageNumber { get; set; }

        public DatabaseContext? context;

        public List<Post>? PostsList { get; set; }
        public int numOfPosts { get; set; }
        public int numOfPages { get; set; }
        public int pageNumberInt { get; set; }

        string Type { get; } = "patchnote";

        protected override async Task OnInitializedAsync()
        {
            int number;
            bool result = Int32.TryParse(pageNumber, out number);
            pageNumberInt = number;
            context ??= await PatchnoteDataContextFactory.CreateDbContextAsync();
            if (context != null)
            {
                var list = await context.Posts.ToListAsync();
                PostsList = list.FindAll(x => x.Type == "forum");
                PostsList = PostsList.OrderByDescending(p => p.ReleaseDate).ToList();
                numOfPosts = PostsList.Count;
                numOfPages = (int)Math.Ceiling(numOfPosts / 10.0);

            }
        }

        public async Task RemovePost(Post patchnote)
        {
            context ??= await PatchnoteDataContextFactory.CreateDbContextAsync();
            if (context != null)
            {
                if (patchnote != null)
                {
                    context.Posts.Remove(patchnote);
                    await context.SaveChangesAsync();
                }
            }
            await JS.InvokeVoidAsync("eval", "window.location.reload();");
        }

        public void Return() { }

        public void OpenPage(Post post)
        {
            NavigationManager.NavigateTo($"/edit/post-{post.Id}", true);
        }

        public void OpenPageCreateNewPatchnote(string type)
        {
            NavigationManager.NavigateTo($"/create-post/{type}", true);
        }
    }
}
