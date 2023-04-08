using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebQuiz.Data;
using WebQuiz.Models;
using WebQuiz.Models.Service;

namespace WebQuiz.Controllers
{
    [Serializable]
    public class AdminPageController : Controller
    {
        private readonly IService service;

        public AdminPageController(IConfiguration configuration, IService service)
        {
            this.service = service;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<User> users = await service.LoadAdminPageAsync();
            if (users.Count > 0)
                return View(users);

            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Index(string? searchString = null)
        {
            var users = service.FindUserAsync(searchString);
            if (users.Result.Count() > 0)
                return View(await users);

            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> AppointModerator(int userId)
        {
            if(await service.NewRoleToUserAsync(userId, 1))    // 1 - назначить модератора
            {
                return RedirectToAction("Index");
            }
            else
                return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> RemoveModerator(int userId)
        {
            if (await service.NewRoleToUserAsync(userId, 0))    // 0 - снять модератора
            {
                return RedirectToAction("Index");
            }
            else
                return RedirectToAction("Index", "Home");
        }
    }
}
