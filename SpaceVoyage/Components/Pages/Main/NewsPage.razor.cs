using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using SpaceVoyage.Data;
using System.Diagnostics;


namespace SpaceVoyage.Components.Pages.Main
{
    public partial class NewsPage
    {
        [Parameter]
        public string pageNumber { get; set; }

        public DatabaseContext? context;

        public List<Post>? PatchnotesList { get; set; }
        public int numOfPatchnotes { get; set; }
        public int numOfPages { get; set; }
        public int pageNumberInt { get; set; }

        protected override async Task OnInitializedAsync()
        {
            int number;
            bool result = Int32.TryParse(pageNumber, out number);
            pageNumberInt = number;
            context ??= await PatchnoteDataContextFactory.CreateDbContextAsync();
            if (context != null)
            {
                PatchnotesList = await context.Posts.ToListAsync();
                PatchnotesList = PatchnotesList.OrderByDescending(p => p.ReleaseDate).ToList();
                numOfPatchnotes = PatchnotesList.Count;
                numOfPages = (int)Math.Ceiling(numOfPatchnotes / 10.0);

            }
        }

        public void Return() { }
    }
}
