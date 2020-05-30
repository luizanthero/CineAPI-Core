using System.ComponentModel.DataAnnotations;

namespace CineAPI.ViewModels
{
    public class UserAuthenticate
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}