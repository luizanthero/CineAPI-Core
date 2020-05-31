using System.ComponentModel.DataAnnotations;

namespace CineAPI.ViewModels
{
    public class UserAuthenticateViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}