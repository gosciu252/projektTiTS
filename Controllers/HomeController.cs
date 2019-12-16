using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AspNetIdentitySample.Models;
using Microsoft.AspNetCore.Authorization;
using AspNetIdentitySample.Services;
using Microsoft.AspNetCore.Identity;
using AspNetIdentitySample.Models.Database;

namespace AspNetIdentitySample.Controllers
{
    /// <summary>
    /// Główny kontroler aplikacji. Zawiera wszystkie wykorzystywane w aplikacji akcje.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TokenGenerator _tokenGenerator;
        private readonly TokensService _tokensService;
        private readonly CandidatesService _candidatesService;
        private readonly UsersService _usersService;
        private readonly UserManager<ApplicationUser> _userManager;
        private const int _tokenLength = 50;

        /// <summary>
        /// Konstruktor klasy HomeController. Przez parametry konstruktora wstrzykujemy wszystkie potrzebne klasy,
        /// które zostały zarejestrowane w klasie Startup.
        /// </summary>
        public HomeController(
            ILogger<HomeController> logger,
            TokenGenerator tokenGenerator,
            TokensService tokensService,
            CandidatesService candidatesService,
            UsersService usersService,
            UserManager<ApplicationUser> userManager
        )
        {
            _logger = logger;
            _tokenGenerator = tokenGenerator;
            _tokensService = tokensService;
            _candidatesService = candidatesService;
            _usersService = usersService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Akcja służąca do pokazania informacji o wygenerowanym tokenie
        [Authorize]
        public IActionResult GenerateToken()
        {
            var userId = _userManager.GetUserId(User);
            var userTokenInfo = _tokensService.GetTokenInfoForUser(userId);

            var model = new GenerateTokenViewModel
            {
                AlreadyGenerated = userTokenInfo.TokenGenerated
            };

            return View(model);
        }

        // Akcja POST służąca do wygenerowania tokenu, zapisania na bazie danych i zwrócenia użytkownikowi.
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GenerateToken(GenerateTokenViewModel postModel)
        {
            var userId = _userManager.GetUserId(User);
            var userTokenInfo = _tokensService.GetTokenInfoForUser(userId);

            if (userTokenInfo.TokenGenerated)
            {
                return BadRequest("Token dla bieżącego użytkownika był już generowany wcześniej");
            }

            var generatedToken = _tokenGenerator.GenerateToken(_tokenLength);
            _tokensService.AddTokenForUser(generatedToken);
            await _usersService.SetTokenGeneratedForUser(userId);

            var model = new GenerateTokenViewModel
            {
                Token = generatedToken
            };

            return View(model);
        }

        /// <summary>
        /// Widok umożliwiający podanie tokenu i przejście do głosowania.
        /// </summary>
        public IActionResult LoginToVote()
        {
            return View();
        }

        /// <summary>
        /// Akcja POST przyjmująca token podany przez użytkownika i sprawdzająca czy token znajduje
        /// się w bazie danych. Jeżeli tak, nastąpi przekierowanie do głosowania. Jeżeli nie,
        /// użytkownik zobaczy wiadomość o błędzie.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult LoginToVote(LoginToVoteViewModel model)
        {
            var token = _tokensService.FindTokenByValue(model.Token);

            if(token == null)
            {
                ModelState.AddModelError("Token", "Podano nieprawidłowy token");
                return View(model);
            }

            // Przekierowanie do strony głosowania z przekazaniem tokenu
            return RedirectToAction(nameof(Vote), new { token = model.Token });
        }

        /// <summary>
        /// Widok głosowania.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public IActionResult Vote(string token)
        {
            var model = new VoteViewModel
            {
                Candidates = _candidatesService.GetList()
            };

            return View(model);
        }

        /// <summary>
        /// Głosowanie - akcja POST. Ponowna weryfikacja tokenu użytkownika i zapisanie głosu w bazie danych.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Vote(string token, VoteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Candidates = _candidatesService.GetList();
                return View(model);
            }

            var foundToken = _tokensService.FindTokenByValue(token);
            if (foundToken == null)
                return BadRequest("Niewłaściwy token");

            _candidatesService.IncreaseCandidateVote(model.CandidateId.Value);
            _tokensService.RemoveToken(foundToken.ID);

            // Przekierowanie do widoku pokazującego wiadomość o pomyślnym głosowaniu
            return RedirectToAction(nameof(VoteSucceeded));
        }

        /// <summary>
        /// Widok pokazujący wiadomość o pomyślnym głosowaniu
        /// </summary>
        /// <returns></returns>
        public IActionResult VoteSucceeded()
        {
            return View();
        }

        /// <summary>
        /// Akcja dostępna tylko dla użytkowników z rolą "Admin". Wyświetla wyniki głosowania.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public IActionResult AdminContent()
        {
            var candidates = _candidatesService.GetList();

            return View(candidates);
        }

        /// <summary>
        /// Domyślny widok błędu wygenerowany przez szablon ASP.NET Core
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
