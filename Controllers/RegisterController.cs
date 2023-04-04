using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebQuiz.Models;
using WebQuiz.Models.Service;

namespace WebQuiz.Controllers
{
    [Serializable]
    public class RegisterController : Controller
    {
        private readonly IService service;

        public RegisterController(IService service)
        {
            this.service = service;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(User user,
                                    [Required(ErrorMessage = "Введите подтверждение пароля")] string confirmPassword)
        
        {
            if (ModelState.IsValid && string.Equals(user.Password, confirmPassword))
            {
                ViewBag.ErrorMessage = await service.RegisterUserAsync(confirmPassword, user);
            }
            else
            {
                ViewBag.ErrorMessage = "Данные введены неверно";
            }
            return View();
        }
    }
}
