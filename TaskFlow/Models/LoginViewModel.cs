using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Models
{
    public class LoginViewModel
    {
        [Required]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
