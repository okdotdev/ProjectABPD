using System.ComponentModel.DataAnnotations;

namespace abcAPI.Models.ViewModels;

public class LoginViewModel
    {
        [Required]
        public string Nickname { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }