using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebQuiz.Data;
using WebQuiz.Models;
using WebQuiz.Models.Service;

namespace WebQuiz.Controllers
{
    [Serializable]
    public class SignInController : Controller
    {
        private readonly IService service;

        public SignInController(IService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(User user)
        {
            if (ModelState.IsValid)
            {
                ViewBag.ErrorMessage = await service.LogInAsync(user);
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else { ViewBag.ErrorMessage = "Данные введены неверно"; }

            return View();
        }
    }
}
