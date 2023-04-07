using System.ComponentModel.DataAnnotations;

namespace WebQuiz.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите имя пользователя")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Длина имени пользователя должна составлять от 2 до 20 символов")]
        [Display(Name = "Имя пользователя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Длина пароля должна составлять от 3 до 20 символов")]
        [DataType(DataType.Password)]
        [Display(Name = "Введите пароль")]
        public string Password { get; set; }
        public int RoleId { get; set; }
    }
}
