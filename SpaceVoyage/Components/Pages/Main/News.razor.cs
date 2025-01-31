using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using SpaceVoyage.Data;
using System.Diagnostics;

namespace SpaceVoyage.Components.Pages.Main
{
    public partial class News
    {
        public bool CreateShowForm { get; set; }
        public bool EditShowForm { get; set; }
        public bool ShowPatchnote { get; set; }

        private PatchnoteDataContext? context;

        [SupplyParameterFromForm]
        public Patchnote? NewPatchnote { get; set; }
        [SupplyParameterFromForm]
        public Patchnote? PatchnoteToUpdate { get; set; }
        public Patchnote? PatchnoteToShow { get; set; }
        public int selectedId { get; set; }
        public List<Patchnote>? PatchnotesList { get; set; }
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
            NewPatchnote = new Patchnote();

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
                    var patchnote = context.PatchNotes.FirstOrDefault(x => x.Title == NewPatchnote.Title);
                    if (patchnote != null)
                    {
                        ErrorMessage = "Patch with this title already exists!";
                        return;
                    }
                    context?.PatchNotes.Add(NewPatchnote);
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
                PatchnotesList = await context.PatchNotes.ToListAsync();
                PatchnotesList = PatchnotesList.OrderByDescending(p => p.ReleaseDate).ToList();

                numOfPatchnotes = PatchnotesList.Count;
                numOfPages = (numOfPatchnotes / 10);
            }
        }

        //Update
        public async Task ShowEditForm(Patchnote patchnote)
        {
            
            context ??= await PatchnoteDataContextFactory.CreateDbContextAsync();
            if ( context != null)
            {
                PatchnoteToUpdate = context.PatchNotes.FirstOrDefault(x => x.Id == patchnote.Id);
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
                    var patchnote = context.PatchNotes.FirstOrDefault(x => x.Title == PatchnoteToUpdate.Title);
                    if (patchnote != null)
                    {
                        ErrorMessage = "Patch with this title already exists!";
                        return;
                    }
                    context.PatchNotes.Update(PatchnoteToUpdate);

                }
                await context.SaveChangesAsync();
            }
            ErrorMessage = string.Empty;
            EditShowForm = false;
        }

        //Delte
        public async Task RemovePatchnote(Patchnote patchnote)
        {
            context ??= await PatchnoteDataContextFactory.CreateDbContextAsync();
            if (context != null)
            {
                if (patchnote != null)
                {
                    context.PatchNotes.Remove(patchnote);
                    await context.SaveChangesAsync();
                }
            }
            await ShowPatchnotes();
        }

        //Show
        public async Task ShowSelectedPatchnote(Patchnote patchnote)
        {
            context ??= await PatchnoteDataContextFactory.CreateDbContextAsync();
            if (context != null)
            {
                PatchnoteToShow = context.PatchNotes.FirstOrDefault(x => x.Id == patchnote.Id);
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
            Console.WriteLine("gg");
            var file = e.File;
            var allowedExtensions = new[] { ".png", ".jpg", ".jpeg" };

            var extension = Path.GetExtension(file.Name).ToLower();
            if (!allowedExtensions.Contains(extension))
            {
                Console.WriteLine("Unsupported format of file!");
                return;
            }

            string uploadPath = Path.Combine("wwwroot", "uploadPictures");
            string fileExtension = Path.GetExtension(e.File.Name);
            string newFileName = $"{Guid.NewGuid()}{fileExtension}";
            string filePath = Path.Combine(uploadPath, file.Name);

            await using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.OpenReadStream().CopyToAsync(fileStream);

            NewPatchnote.FilePath = $"/uploadPictures/{file.Name}";
        }
    }
}
