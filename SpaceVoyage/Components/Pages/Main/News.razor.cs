using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using SpaceVoyage.Data;
using System.Diagnostics;
using System.IO;

namespace SpaceVoyage.Components.Pages.Main
{
    public partial class News
    {
        public bool CreateShowForm { get; set; }
        public bool EditShowForm { get; set; }
        public bool ShowPatchnote { get; set; }

        private DatabaseContext? context;

        [SupplyParameterFromForm]
        public Post? NewPatchnote { get; set; }

        [SupplyParameterFromForm]
        public Post? PatchnoteToUpdate { get; set; }
        public Post? PatchnoteToShow { get; set; }
        public int selectedId { get; set; }
        public List<Post>? PatchnotesList { get; set; }
        public string? ErrorMessage { get; set; }

        public int numOfPatchnotes { get; set; }

        public int currentPageNumber { get; set; }
        
        public int numOfPages { get; set; }

        string Type { get; } = "patchnote";

        protected override async Task OnInitializedAsync()
        {
            StateHasChanged();

            CreateShowForm = false;
            await ShowPatchnotes();
            ErrorMessage = string.Empty;
            currentPageNumber = 0;
        }

 


        //Read
        public async Task ShowPatchnotes()
        {
            context ??= await PatchnoteDataContextFactory.CreateDbContextAsync();
            if (context != null)
            {
                PatchnotesList = await context.Posts.ToListAsync();
                PatchnotesList = PatchnotesList.OrderByDescending(p => p.ReleaseDate).ToList();

                numOfPatchnotes = PatchnotesList.Count;
                numOfPages = (int)Math.Ceiling(numOfPatchnotes / 10.0);
            }
        }

        //Update
        public async Task ShowEditForm(Post patchnote)
        {
            
            context ??= await PatchnoteDataContextFactory.CreateDbContextAsync();
            if ( context != null)
            {
                PatchnoteToUpdate = context.Posts.FirstOrDefault(x => x.Id == patchnote.Id);
                EditShowForm = true;
                selectedId = patchnote.Id;
            }
        }

 

        //Delte
        public async Task RemovePatchnote(Post patchnote)
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
            await ShowPatchnotes();
        }

        //Show
        public async Task ShowSelectedPatchnote(Post patchnote)
        {
            context ??= await PatchnoteDataContextFactory.CreateDbContextAsync();
            if (context != null)
            {
                PatchnoteToShow = context.Posts.FirstOrDefault(x => x.Id == patchnote.Id);
                ShowPatchnote = true;
                selectedId = patchnote.Id;
            }
        }

        public async Task Return()
        {
            ErrorMessage = string.Empty;
            ShowPatchnote = false;
            EditShowForm = false;
            CreateShowForm = false;
            await ShowPatchnotes();
        }


        public void OpenPageCreateNewPatchnote(string type)
        {
            NavigationManager.NavigateTo($"/create-post/{type}", true);
        }

        public void OpenPage(Post post)
        {
            NavigationManager.NavigateTo($"/edit/post-{post.Id}", true);
        }
    }
}
