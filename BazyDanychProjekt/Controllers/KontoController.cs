using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using BazyDanychProjekt.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class KontoController : Controller
{
    private readonly ILogger<KontoController> _logger;
    private readonly UserManager<Uzytkownik> _userManager;
    private readonly SignInManager<Uzytkownik> _signInManager;

    public KontoController(UserManager<Uzytkownik> userManager, SignInManager<Uzytkownik> signInManager, ILogger<KontoController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    //public KontoController() { }


    [HttpGet]
    public IActionResult Logowanie()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Logowanie(LogowanieViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Login, model.Haslo, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.Login);
                HttpContext.Session.SetString("UserId", user.Id);
                HttpContext.Session.SetString("UserName", user.UserName);

                return RedirectToAction("Index", "Hotele");
            }

            ModelState.AddModelError(string.Empty, "Błąd logowania. Spróbuj ponownie.");
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Rejestracja()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Rejestracja(RejestracjaViewModel model)
    {
//        if (ModelState.IsValid)
//        {

            var user = new Uzytkownik { UserName=model.Login, Haslo=model.Haslo, Login = model.Login, Imie = model.Imie, Nazwisko = model.Nazwisko, Rola = "Uzytkownik" };
            var result = await _userManager.CreateAsync(user, model.Haslo);
            _logger.LogError("Weszlismy");
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                _logger.LogError("Zarejestrowano");
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
                _logger.LogError($"Błąd rejestracji: {error.Description}");
            }
  //      }

        return View(model);
    }

    public async Task<IActionResult> Wyloguj()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
