using System.Collections.Generic;

namespace CineAPI.ViewModels
{
    public class UserAuthenticateTokenViewModel
    {
        public string Token { get; set; }

        public ICollection<int> Roles { get; set; }
    }
}