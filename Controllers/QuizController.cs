using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebQuiz.Data;
using WebQuiz.Models;

namespace WebQuiz.Controllers
{
    [Serializable]
    public class QuizController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        readonly DbContextOptionsBuilder<AppDbContext> _optionsBuilder;
        private readonly ILogger logger;

        public QuizController(IConfiguration configuration, ILogger<QuizController> logger)
        {
            _configuration = configuration;
            _optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            _optionsBuilder.UseNpgsql(_connectionString);

            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Index(int type, bool returnedFromPage = false)
        {
            try
            {
                if (!returnedFromPage)      // переход выполнен с HomeController - Index. Запускаем викторину. Иначе - была нажата
                                            // кнопка "Назад" ( выполняется возврат к прошлому вопросу ) 
                    QuizState.StartQuiz(_optionsBuilder.Options, type);

                return View();
            }
            catch (Exception ex)
            {
                logger.LogError("Error in [QuizController].GET_INDEX\nException: {0}", ex.ToString());
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult Index(string answer)
        {
            try
            {
                if (QuizState.QuizGoesOn(answer))  // изменить название функции
                    return View();
                else
                    return RedirectToAction("EndQuiz");
            }
            catch(Exception ex)
            {
                logger.LogError("Error in [QuizController].POST_INDEX\nException: {0}", ex.ToString());
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult GetToPrevQuestion()
        {
            if (QuizState.CurrentQuestionIndex != 0)
                QuizState.CurrentQuestionIndex -= 1;
            return RedirectToAction("Index", new { returnedFromPage = true });
        }

        [HttpGet]
        public async Task<IActionResult> EndQuiz() 
        {
            if(!QuizState.ResultIsShown)
            {
                await Result.CheckAnswers(_optionsBuilder.Options);
            }

            return View();
        }

        [HttpPost]
        public IActionResult ToHome()
        {
            return RedirectToAction("Index", "Home");
        }

    }
}
