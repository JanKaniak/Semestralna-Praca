using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using SpaceVoyage.Data;
using System.Diagnostics;
using System.IO;
using System.Security.Claims;

namespace SpaceVoyage.Components.Pages.Main
{
    public partial class Forum
    {
        public bool CreateShowForm { get; set; }
        public bool EditShowForm { get; set; }
        public bool ShowPost { get; set; }

        private DatabaseContext? context;

        [SupplyParameterFromForm]
        public Post? NewPost { get; set; }
        [SupplyParameterFromForm]
        public Post? PostToUpdate { get; set; }
        public Post? PostToShow { get; set; }
        public int selectedId { get; set; }
        public List<Post>? PostsList { get; set; }
        public string? ErrorMessage { get; set; }

        public int numOfPosts { get; set; }

        public int currentPageNumber { get; set; }

        public int numOfPages { get; set; }

        private string? UserId { get; set; }
        public List<User>? UserList { get; set; }

        public string Type { get; } = "forum";

        protected override async Task OnInitializedAsync()
        {
            CreateShowForm = false;
            await ShowPosts();
            ErrorMessage = string.Empty;
            currentPageNumber = 0;

            
        }




        //Read
        public async Task ShowPosts()
        {
            context ??= await PatchnoteDataContextFactory.CreateDbContextAsync();
            if (context != null)
            {
                PostsList = await context.Posts.ToListAsync();
                PostsList = PostsList.OrderByDescending(p => p.ReleaseDate).ToList();

                numOfPosts = PostsList.Count;
                numOfPages = (int)Math.Ceiling(numOfPosts / 10.0);
            }
        }

    

        //Delte
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
            await ShowPosts();
        }

        //Show
        public async Task ShowSelectedPost(Post patchnote)
        {
            context ??= await PatchnoteDataContextFactory.CreateDbContextAsync();
            if (context != null)
            {
                PostToShow = context.Posts.FirstOrDefault(x => x.Id == patchnote.Id);
                ShowPost = true;
                selectedId = patchnote.Id;
            }
        }

        public async Task Return()
        {
            ErrorMessage = string.Empty;
            ShowPost = false;
            EditShowForm = false;
            CreateShowForm = false;
            await ShowPosts();
        }

        public bool CreatedPost(Post post)
        {
            if (!string.IsNullOrWhiteSpace(UserId))
            {
                Logger.LogInformation(UserId);
                Logger.LogInformation(post.UserId.ToString());

                if (UserId == post.UserId.ToString())
                {
                    return true;
                }
                if (UserList.Find(x => x.UserId == Int32.Parse(UserId)).UserRole == "Administrator")
                {
                    return true;
                }
            }
            return false;
        }

        public void OpenPage(Post post)
        {
            NavigationManager.NavigateTo($"/edit/post-{post.Id}", true);
        }

        public void OpenPageCreateNewPost(string type)
        {
            NavigationManager.NavigateTo($"/create-post/{type}", true);
        }

    }
}
