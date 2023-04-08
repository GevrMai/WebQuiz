using Microsoft.AspNetCore.Mvc;
using WebQuiz.Models;
using WebQuiz.Models.Service;

namespace WebQuiz.Controllers
{
    [Serializable]
    public class AccountController : Controller
    {
        private readonly IService service;

        public AccountController(IService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? errorMessage = null)
        {
            if (CurrentUser._CurrentUser == null)
                return RedirectToAction("Index", "Home", new { authorized = false });

            ViewBag.ErrorMessage = errorMessage;
            return View(await service.LoadUserResultsAsync());
        }

        [HttpPost]
        public IActionResult SignOut()
        {
            CurrentUser._CurrentUser = null;
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> ChangePasswordAsync(string newPassword, string newPasswordConfirm)
        {
            try
            {
                await service.ChangePasswordAsync(newPassword, newPasswordConfirm);
                return RedirectToAction("Index", new {errorMessage = "Пароль успешно изменен!" });

            }
            catch
            {
                return RedirectToAction("Index",
                    new { errorMessage = "Произошла ошибка при изменении пароля.Длина пароля должна составлять от 3 символов" });
            }
        }
    }
}
