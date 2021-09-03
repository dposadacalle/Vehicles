using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vehicles.API.Helpers;
using Vehicles.API.Models;

namespace Vehicles.API.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;

        public AccountController(IUserHelper userHelper)
        {
            _userHelper = userHelper;
        }

        public IActionResult Login()
        {
            // If the user it logging
            if (User.Identity.IsAuthenticated)
            {
                // Redirect to View Index into the HomeController
                return RedirectToAction(nameof(Index), "Home");
            }

            // Return a view to new LoginViewModel
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Valide if the model have all the conditions del LoginViewModel 
            if (ModelState.IsValid)
            {
                var result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUri"))
                    {
                        return Redirect(Request.Query["ReturnUri"].First());
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Email ó contraseña incorrectos.");
            }

            // If not is validate
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
