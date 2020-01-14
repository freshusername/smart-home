using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Core.Model;
using Infrastructure.Business.DTOs;
using Infrastructure.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using smart_home_web.Models;

namespace smart_home_web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMapper _mapper;
        public UserManager<AppUser> UserManager { get; private set; }
        public SignInManager<AppUser> SignInManager { get; private set; }

        private readonly IAuthenticationManager _authenticationManager;

        private readonly IEmailSender _emailSender;

        public AccountController(IAuthenticationManager authenticationManager, IEmailSender emailSender, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMapper mapper)

        {
            _authenticationManager = authenticationManager;
            _emailSender = emailSender;
            UserManager = userManager;
            SignInManager = signInManager;
            _mapper = mapper;
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _mapper.Map<RegisterViewModel, UserDTO>(model);
            var result = await _authenticationManager.Register(user);
            if (result.Succeeded)
            {
                var confrirmaParam = await _authenticationManager.GetEmailConfirmationToken(user.Email);
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = confrirmaParam.UserId, code = confrirmaParam.Code }, protocol: HttpContext.Request.Scheme);


                //await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                //    $"Your information has been sent successfully. In order to complete your registration, please click the confirmation link in the email that we have sent to you.: <a href='{callbackUrl}'>link</a>");
                return View("EmailConfirmation");
            }
            else
                ModelState.AddModelError(result.Property, result.Message);

            return View(model);

        }


        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var loginModel = _mapper.Map<LoginViewModel, UserDTO>(model);
            var identity = await _authenticationManager.Login(loginModel);

            if (!identity.Succeeded)
            {
                ModelState.AddModelError(identity.Property, identity.Message);
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _authenticationManager.Logout();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var confirmParam = await _authenticationManager.GetPasswordConfirmationToken(model.Email);
                if (confirmParam == null)
                    ModelState.AddModelError("", "User with this email is not exist");
                else
                {
                    var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = confirmParam.UserId, code = confirmParam.Code }, protocol: HttpContext.Request.Scheme);


                    await _emailSender.SendEmailAsync(model.Email, "Reset your password",
                       $"Please reset your password by clicking here.: <a href='{callbackUrl}'>link</a>");

                    return View("ForgotPasswordConfirmation");
                }
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null, string userId = null)
        {
            if (userId == null || code == null)
                return View("Error");
            else
                return View("ResetPassword");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
                return View("ResetPasswordConfirmation");

            var result = await UserManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
                return View("ResetPasswordConfirmation");

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            else
                return View("Error");
        }


        [AllowAnonymous]
        public IActionResult SignInGoogle()
        {
            string redirectUrl = Url.Action("GoogleResponse", "Account");
            var properties = SignInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await _authenticationManager.GoogleAuthentication();
            if (result == null)
                return RedirectToAction(nameof(Login));

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            return View("AccessDenied");
        }

    }
}
