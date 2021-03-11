using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StoreMVC.Models
{
    public class CreateUserViewModel
    {
        [DisplayName("Username")]
        [Required]
        public string UserName { get; set; }

        [DisplayName("Is this user a manager?")]
        public bool IsManager { get; set; }
    }
}
