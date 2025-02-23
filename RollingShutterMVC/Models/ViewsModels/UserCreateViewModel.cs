using System.ComponentModel.DataAnnotations;

namespace RollingShutterMVC.Models.ViewsModels
{
    public class UserCreateViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı gereklidir")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Şifre gereklidir")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Şifre onayı gereklidir")]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
        public string? Role { get; set; }  
    }
}
