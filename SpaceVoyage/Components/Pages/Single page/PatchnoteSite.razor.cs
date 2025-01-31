using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using SpaceVoyage.Data;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.Query.Internal.ExpressionTreeFuncletizer;

namespace SpaceVoyage.Components.Pages.Single_page
{
    public partial class PatchnoteSite
    {
        [Parameter]
        public string Id { get; set; }
        public PatchnoteDataContext? context;
        public List<Patchnote>? PatchnotesList { get; set; }

        
        public Patchnote? PatchnoteToShow { get; set; }

        protected override async Task OnInitializedAsync()
        {
            context ??= await PatchnoteDataContextFactory.CreateDbContextAsync();
            if (context != null)
            {
                PatchnoteToShow = context.PatchNotes.FirstOrDefault(x => x.Id == Int32.Parse(Id));
                PatchnotesList = await context.PatchNotes.ToListAsync();
            }

        }

        public async void Return()
        {
            context ??= await PatchnoteDataContextFactory.CreateDbContextAsync();
            if (context != null && PatchnotesList != null && PatchnoteToShow != null)
            {
                int position = PatchnotesList.IndexOf(PatchnoteToShow);
                int numOfPages = PatchnotesList.Count / 10;
                if (position <= 10)
                {
                    NavigationManager.NavigateTo("/news", true);
                }
                for (int i = 1; i < numOfPages; i++)
                {
                    int border = 20;
                    if (position <= border)
                    {
                        NavigationManager.NavigateTo($"/news-{i}", true);
                    }
                    border += 10;
                }
            }
            NavigationManager.NavigateTo("/news", true);
        }










    }
}
