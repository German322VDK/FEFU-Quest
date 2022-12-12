using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FEFU_Quest.ViewModels
{
    public class RegisterUserViewModel
    {
        public string Email { get; set; }

        [Required(ErrorMessage = "Логин обязателен и не должен использоваться другими"), MaxLength(256)]
        [Display(Name = "Логин пользователя")]
        [RegularExpression(@"^[A-Za-z0-9]{1,40}$",
         ErrorMessage = "Только английские буквы и цыфры")]
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }
        
        public string Patr { get; set; }

        public string Dormitory { get; set; }

        public string UniverGroup { get; set; }

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
