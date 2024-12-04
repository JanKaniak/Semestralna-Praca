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
        protected override async Task OnInitializedAsync()
        {
            CreateShowForm = false;
            await ShowPatchnotes();
            ErrorMessage = string.Empty;
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
                    if (string.IsNullOrEmpty(NewPatchnote.Title) || string.IsNullOrEmpty(NewPatchnote.Description) || NewPatchnote.testovaciStlpec == null)
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
                    if (string.IsNullOrEmpty(PatchnoteToUpdate.Title) || string.IsNullOrEmpty(PatchnoteToUpdate.Description) || PatchnoteToUpdate.testovaciStlpec == null)
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
            ShowPatchnote = false;
            EditShowForm = false;
            CreateShowForm = false;
            await ShowPatchnotes();
        }
    }
}
