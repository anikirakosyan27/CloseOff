using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using DataModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CloseOff.Controllers
{
    public class AccountController : BaseController
    {
        private readonly SignInManager<User> _signInManager;
        public AccountController(IUnitOfWork unitOfWork,
         UserManager<User> userManager,
         SignInManager<User> signInManager)
         : base(unitOfWork, userManager)
        {
            _signInManager = signInManager;

        }
        [TempData]
        public string ErrorMessage { get; set; }
        #region Login
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            ViewBag.HideFooter = true;
            ViewData["ReturnUrl"] = returnUrl;
            return View("Login", new LoginModel { RememberMe = true });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                ViewBag.HideFooter = true;
                return View(model);
            }
            //if (!user.EmailConfirmed)
            //{
            //    return RedirectToAction("GetConfirmationEmail", "Account", new { type = (int)ConfirmationType.ConfirmEmail });
            //}
            try
            {
                var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);
                if (!signInResult.Succeeded)
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                    ViewBag.HideFooter = true;
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Invalid login attempt.{ex.Message}");
                ViewBag.HideFooter = true;
                return View(model);
            }
            return RedirectToAction("Profile", "User");
        }

        #endregion

        #region Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewBag.HideFooter = true;
            ViewData["ReturnUrl"] = returnUrl;
            RegisterModel model = new RegisterModel();
            return View("Reg", model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                //  var user = _mapper.Map<User>(model);
                var existUser = await _userManager.FindByEmailAsync(model.Email);
                if (existUser != null)
                {
                    ModelState.AddModelError("", "Email already exists");
                    ViewBag.HideFooter = true;
                    return View(model);
                }
                var user = new User { UserName = model.Email, Email = model.Email,IsActive=true,CreateDate=DateTime.Now, FirstName = model.FirstName, LastName = model.LastName };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                AddErrors(result);
            }
            ViewBag.HideFooter = true;
            // If we got this far, something failed, redisplay form
            return View(model);
        }
        #endregion
       

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        #endregion
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }
    }
}
