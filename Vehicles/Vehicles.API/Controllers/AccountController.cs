using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vehicles.API.Data;
using Vehicles.API.Data.Entities;
using Vehicles.API.Helpers;
using Vehicles.API.Models;
using Vehicles.Common.Enums;

namespace Vehicles.API.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IBlobHelper _blobHelper;

        public AccountController(IUserHelper userHelper, DataContext context, ICombosHelper combosHelper, 
            IBlobHelper blobHelper)
        {
            _userHelper = userHelper;
            _context = context;
            _combosHelper = combosHelper;
            _blobHelper = blobHelper;
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

        public IActionResult NotAuthrized()
        {
            return View();
;       }

        public IActionResult Register()
        {
            AddUserViewModel model = new AddUserViewModel
            {
                DocumentTypes = _combosHelper.GetComboDocumentTypes()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                }

                User user = await _userHelper.AddUserAsync(model, imageId, UserType.User);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Este correo ya está siendo usado por otro usuario.");
                    model.DocumentTypes = _combosHelper.GetComboDocumentTypes();
                    return View(model);
                }

                LoginViewModel loginViewModel = new LoginViewModel
                {
                    Password = model.Password,
                    RememberMe = false,
                    Username = model.Username
                };

                var result2 = await _userHelper.LoginAsync(loginViewModel);
                if (result2.Succeeded) return RedirectToAction("Index", "Home");

            }

            model.DocumentTypes = _combosHelper.GetComboDocumentTypes();
            return View(model);
        }

    }
}
