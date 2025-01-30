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

        
        public Patchnote? PatchnoteToShow { get; set; }

        protected override async Task OnInitializedAsync()
        {
            context ??= await PatchnoteDataContextFactory.CreateDbContextAsync();
            if (context != null)
            {
                PatchnoteToShow = context.PatchNotes.FirstOrDefault(x => x.Id == Int32.Parse(Id));
            }

        }

        public void Return()
        {
            NavigationManager.NavigateTo("/news", true);
        }










    }
}
