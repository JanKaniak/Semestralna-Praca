using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SpaceVoyage.Data;

namespace SpaceVoyage.Components.Layout
{
    public partial class NavMenu
    {
        private DatabaseContext? context;

        public string UserName { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user != null && user.Identity != null && user.Identity.Name != null)
            {
                UserName = user.Identity.Name;
            }
        }
    }
}
