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

        protected override async Task OnInitializedAsync()
        {
            CreateShowForm = false;
            await ShowPatchnotes();
            ErrorMessage = string.Empty;
            currentPageNumber = 0;
        }

        //Create
        public void ShowCreateForm()
        {
            CreateShowForm = true;
            NewPatchnote = new Post();

        }

        public async Task CreateNewPatchnote()
        {
            context ??= await PatchnoteDataContextFactory.CreateDbContextAsync();
            if (context != null)
            {
                if (NewPatchnote != null)
                {
                    if (string.IsNullOrEmpty(NewPatchnote.Title) || string.IsNullOrEmpty(NewPatchnote.Description))
                    {
                        ErrorMessage = "All fields are required!";
                        return;
                    }
                    var patchnote = context.Posts.FirstOrDefault(x => x.Title == NewPatchnote.Title);
                    if (patchnote != null)
                    {
                        ErrorMessage = "Patch with this title already exists!";
                        return;
                    }
                    NewPatchnote.Type = "patchnote";
                    context?.Posts.Add(NewPatchnote);
                    context?.SaveChangesAsync();

                }
            }
            ErrorMessage = string.Empty;
            CreateShowForm = false;
            await ShowPatchnotes();

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

        public async Task UpdatePatchnote()
        {
            context ??= await PatchnoteDataContextFactory.CreateDbContextAsync();
            if (context != null)
            {
                if (PatchnoteToUpdate != null)
                {
                    if (string.IsNullOrEmpty(PatchnoteToUpdate.Title) || string.IsNullOrEmpty(PatchnoteToUpdate.Description))
                    {
                        ErrorMessage = "All fields are required!";
                        return;
                    }
                    var patchnote = context.Posts.FirstOrDefault(x => x.Title == PatchnoteToUpdate.Title);
                    if (patchnote != null)
                    {
                        ErrorMessage = "Patch with this title already exists!";
                        return;
                    }
                    context.Posts.Update(PatchnoteToUpdate);

                }
                await context.SaveChangesAsync();
            }
            ErrorMessage = string.Empty;
            EditShowForm = false;
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

        private async Task LoadFile(InputFileChangeEventArgs e)
        {
            var file = e.File;
            var allowedExtensions = new[] { ".png", ".jpg", ".jpeg" };

            var extension = Path.GetExtension(file.Name).ToLower();
            if (!allowedExtensions.Contains(extension))
            {
                ErrorMessage = "Unsupported format of file!";
                return;
            }

            string uploadPath = Path.Combine("wwwroot", "uploadPictures");
            string fileName = Path.GetExtension(e.File.Name);
            string newFileName = $"{Guid.NewGuid()}{fileName}";
            string filePath = Path.Combine(uploadPath, newFileName);

            await using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.OpenReadStream().CopyToAsync(fileStream);

            if (NewPatchnote != null)
            {
                NewPatchnote.FilePath = $"{newFileName}";
            }
            else if (PatchnoteToUpdate != null) { 
                PatchnoteToUpdate.FilePath = $"{newFileName}";
            }
                
        }

        public async Task RemoveImage(Post patchnote)
        {
            if (!string.IsNullOrWhiteSpace(patchnote.FilePath))
            {
                var filePath = Path.Combine("wwwroot", "uploadPictures", patchnote.FilePath);
                Logger.LogInformation(filePath);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                patchnote.FilePath = string.Empty;
                context ??= await PatchnoteDataContextFactory.CreateDbContextAsync();
                var patchnoteToChange = context.Posts.FirstOrDefault(x => x.Id == patchnote.Id);
                if (context != null)
                {
                    if (patchnoteToChange != null)
                    {
                        patchnoteToChange.FilePath = patchnote.FilePath;
                        patchnoteToChange.FilePath = null;
                        await context.SaveChangesAsync();
                    }
                }
                imageUploaded = false;
                await InvokeAsync(StateHasChanged);
            }
        }
    }
}
