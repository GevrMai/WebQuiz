using Microsoft.AspNetCore.Mvc;
using WebQuiz.Models;
using WebQuiz.Models.RepositoryIntefraces;
using WebQuiz.Models.Service;

namespace WebQuiz.Controllers
{
    public class OffersController : Controller
    {
        private readonly IOfferRepository repository;
        private readonly IService service;

        public OffersController(IOfferRepository repository, IService service)
        {
            this.repository = repository;
            this.service = service;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                List<Offer> offers = repository.GetAllAsync().Result.OrderByDescending(x => x.CreatedDate).ToList();

                ViewBag.UserRoles = await service.GetRolesOfUsersAsync(offers); // модераторы могут удалять комментарии
                return View(offers);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(string offerField)
        {
            await service.PostOfferAsync(offerField);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteOffer(int offerId)
        {
            await repository.DeleteAsync(offerId);
            return RedirectToAction("Index");
        }
    }
}
