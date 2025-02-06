using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SpaceVoyage.Data;
using System.Linq;

namespace SpaceVoyage.Components.Pages.Single_page
{
    public partial class PostEdit
    {
        [Parameter]
        public string Id { get; set; }

        private DatabaseContext? context;

        [SupplyParameterFromForm]
        public Post? PostToUpdate { get; set; }


        public List<Post>? PostsList { get; set; }
        public string? ErrorMessage { get; set; }


        protected override async Task OnInitializedAsync()
        {
            context ??= await PatchnoteDataContextFactory.CreateDbContextAsync();
            if (context != null && context.Posts != null)
            {
                PostToUpdate = context.Posts.FirstOrDefault(x => x.Id == Int32.Parse(Id));
                PostsList = await context.Posts.ToListAsync();
            }

        }

        public async Task UpdatePatchnote()
        {
            context ??= await PatchnoteDataContextFactory.CreateDbContextAsync();
            if (context != null && context.Posts != null)
            {
                if (PostToUpdate != null)
                {
                    if (string.IsNullOrEmpty(PostToUpdate.Title) || string.IsNullOrEmpty(PostToUpdate.Description))
                    {
                        ErrorMessage = "All fields are required!";
                        return;
                    }
                    var patchnote = context.Posts.FirstOrDefault(x => x.Title == PostToUpdate.Title);
                    if (patchnote != null)
                    {
                        ErrorMessage = "Patch with this title already exists!";
                        return;
                    }
                    PostToUpdate.ReleaseDate = DateTime.Now;
                    context.Posts.Update(PostToUpdate);

                }
                await context.SaveChangesAsync();
            }
            ErrorMessage = string.Empty;
            Return();
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

            if (PostToUpdate != null)
            {
                PostToUpdate.FilePath = $"{newFileName}";
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
            }
        }

        public void Return()
        {
            if (PostToUpdate.Type == "forum")
            {
                NavigationManager.NavigateTo("/forum", true);
            } else
            {
                NavigationManager.NavigateTo("/news", true);
            }
            
        }
    }
}
