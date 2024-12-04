using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;

namespace SpaceVoyage.Components.Pages.Account
{
    public partial class Logout
    {
        [CascadingParameter]
        public HttpContext HttpContext { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync();
                NavigationManager.NavigateTo("/logout", true);
            }
        }
    }
}
