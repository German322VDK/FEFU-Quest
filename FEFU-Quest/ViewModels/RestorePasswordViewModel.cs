using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FEFU_Quest.ViewModels
{
    public class RestorePasswordViewModel
    {
        [Required]
        [HiddenInput]
        public string Email { get; set; }

        [Required]
        [HiddenInput]
        public string GeneratedToken { get; set; }


        [Required(ErrorMessage = "Пароль обязателен")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Подтверждение пароля обязательно")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare(nameof(Password), ErrorMessage = "Пароли не совпали")]
        public string PasswordConfirm { get; set; }
    }
}
