using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SpaceVoyage.Data;
using System.Linq;

namespace SpaceVoyage.Components.Pages.Single_page
{
    public partial class PostCreate
    {
        [Parameter]
        public string? Type { get; set; }

        private DatabaseContext? context;

        [SupplyParameterFromForm]
        public Post? NewPatchnote { get; set; } = new Post();

        public List<Post>? PostsList { get; set; }
        public string? ErrorMessage { get; set; }

        public int numOfPatchnotes { get; set; }

        public int currentPageNumber { get; set; }

        public int numOfPages { get; set; }

        protected override async Task OnInitializedAsync()
        {
            context ??= await PatchnoteDataContextFactory.CreateDbContextAsync();
            if (context != null && context.Posts != null)
            {
                PostsList = await context.Posts.ToListAsync();
            }

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
                    NewPatchnote.Type = Type;
                    context?.Posts.Add(NewPatchnote);
                    context?.SaveChangesAsync();

                }
            }
            ErrorMessage = string.Empty;
            Return();

        }

        public void Return()
        {
            if (Type == "forum")
            {
                NavigationManager.NavigateTo("/forum", true);
            }
            else
            {
                NavigationManager.NavigateTo("/news", true);
            }

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

        }

        public async Task RemoveImage(Post patchnote)
        {
            if (!string.IsNullOrWhiteSpace(patchnote.FilePath))
            {
                var filePath = Path.Combine("wwwroot", "uploadPictures", patchnote.FilePath);
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
                await InvokeAsync(StateHasChanged);
            }
        }
    }
}
