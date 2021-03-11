using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StoreMVC.Models
{
    public class LoginUserViewModel
    {
        [DisplayName("Username")]
        [Required]
        public string UserName { get; set; }
    }
}