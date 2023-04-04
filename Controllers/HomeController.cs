using Microsoft.AspNetCore.Mvc;
using WebQuiz.Models;
using WebQuiz.Models.RepositoryIntefraces;
using WebQuiz.Models.Service;
using static WebQuiz.Models.QuizState;

namespace WebQuiz.Controllers
{
    [Serializable]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IResultRepository resultRepository;
        private readonly IService service;
        public HomeController(ILogger<HomeController> logger, IResultRepository resultRepository, IService service)
        {
            _logger = logger;
            this.resultRepository = resultRepository;
            this.service = service;
        }

        // загрузка главной страницы
        [HttpGet]
        public async Task<IActionResult> Index(bool authorized = true)
        {
            if (!authorized)
            {
                ViewBag.ErrorMessage = "Выполните вход в аккаунт!";
            }
            try
            {
                return View(await resultRepository.GetTop10ResultsAsync());
            }
            catch(Exception ex)
            {
                var response = "[GET] Exception in: Controller: " + nameof(HomeController) + "; Action: " + nameof(Index) + ";\n" + ex;
                _logger.LogError(response);

                return View(new List<Result>());
            }

        }

        // клик на кнопку "флаги", начало викторины
        public IActionResult Flags()
        {
            if (CurrentUser._CurrentUser != null)     // пользователь авторизован
            {
                QuizState.Type = QuizType.Flags;                            // тип викторины
                return RedirectToAction("Index", "Quiz", new { type = 1 }); // направляем на страницу с вопросом, тип 1 - исп. сущность "страна"
            }
            else                                                                // пользователь не авторизован
            {
                return RedirectToAction("Index", new {authorized = false});     // перенаправляем на эту же страницу, появляется ошибка
            }
            
        }

        // клик на кнопку "границы", то же самое, что и в Flags()
        public IActionResult Borders()
        {
            if (CurrentUser._CurrentUser != null)
            {
                QuizState.Type = QuizType.Borders;
                return RedirectToAction("Index", "Quiz", new { type = 1 });
            }
            else
            {
                return RedirectToAction("Index", new { authorized = false });
            }
        }

        // клик на кнопку "правители", то же самое, кроме типа. Тип 2 - сущность "правитель"
        public IActionResult Rulers()
        {
            if (CurrentUser._CurrentUser != null)
            {
                QuizState.Type = QuizType.Rulers;
                return RedirectToAction("Index", "Quiz", new { type = 2 });
            }
            else
            {
                return RedirectToAction("Index", new { authorized = false });
            }
        }

        // переход на представление с инфо о приложении/вар-ами связи
        [HttpGet]
        public IActionResult InfoNContact()
        {
            return View();
        }

        // переход на представление Statistics(все ответы пользователей)
        [HttpGet]
        public async Task<IActionResult> Statistics() 
        {
            return View(await service.LoadStatisticsAsync());
        }

        // пользователь использует сортировку или поиск
        [HttpPost]
        public async Task<IActionResult> Statistics(string searchString, string firstSortParameter, string secondSortParameter)
        {
            return View(await service.LoadStatisticsAsync(searchString, firstSortParameter, secondSortParameter));
        }
        // переход на страницу с рейтингом игроков
        [HttpGet]
        public async Task<IActionResult> Players()
        {
            return View(await service.FormUserRatingAsync());
        }
        //выполняется поиск и сортировка
        [HttpPost]
        public async Task<IActionResult> Players(string searchString, string sortParameter)
        {
            return View(await service.FormUserRatingAsync(searchString, sortParameter));
        }
        //переход на страницу со статистикой стран
        [HttpGet]
        public async Task<IActionResult> CountriesStats()
        {
            return View(await service.CountriesStatsAsync());
        }
        //используется сортировка и поиск
        [HttpPost]
        public async Task<IActionResult> CountriesStats(string searchString, string sortParameter)
        {
            return View(await service.CountriesStatsAsync(searchString, sortParameter));
        }
        // переход на страницу со статистикой правителей
        [HttpGet]
        public async Task<IActionResult> RulersStats()
        {
            return View(await service.RulersStatsAsync());
        }
        //используется поиск и сортировка
        [HttpPost]
        public async Task<IActionResult> RulersStats(string searchString, string sortParameter)
        {
            return View(await service.RulersStatsAsync(searchString, sortParameter));
        }
    }
}