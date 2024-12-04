using Microsoft.EntityFrameworkCore;
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

        public Patchnote? NewPatchnote { get; set; }
        public Patchnote? PatchnoteToUpdate { get; set; }
        public Patchnote? PatchnoteToShow { get; set; }
        public int selectedId { get; set; }
        public List<Patchnote>? PatchnotesList { get; set; }
        public string? ErrorMessage { get; set; }
        protected override async Task OnInitializedAsync()
        {
            CreateShowForm = false;
            await ShowPatchnotes();
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

            if (NewPatchnote != null)
            {
                context?.PatchNotes.Add(NewPatchnote);
                context?.SaveChangesAsync();
                
            }
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
            PatchnoteToUpdate = context.PatchNotes.FirstOrDefault(x => x.Id == patchnote.Id);
            EditShowForm = true;
            selectedId = patchnote.Id;
        }

        public async Task UpdatePatchnote()
        {
            context ??= await PatchnoteDataContextFactory.CreateDbContextAsync();
            if (context != null)
            {
                if (PatchnoteToUpdate != null)
                {
                    context.PatchNotes.Update(PatchnoteToUpdate);

                }
                await context.SaveChangesAsync();
            }
            EditShowForm = false;
        }

        //Delte
        public async Task RemovePatchnote(Patchnote patchnote)
        {
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
            PatchnoteToShow = context.PatchNotes.FirstOrDefault(x => x.Id == patchnote.Id);
            ShowPatchnote = true;
            selectedId = patchnote.Id;
        }

        public async Task Return()
        {
            ShowPatchnote = false;
            await ShowPatchnotes();
        }
    }
}
