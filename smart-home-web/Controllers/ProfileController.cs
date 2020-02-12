using AutoMapper;
using Domain.Core.Model;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using smart_home_web.Models;
using smart_home_web.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smart_home_web.Controllers
{
    [Route("[controller]/[action]")]
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IIconManager _iconManager;
        private readonly IMapper _mapper;

        public ProfileController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IIconManager iconManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _iconManager = iconManager;
            _mapper = mapper;
        }

           
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var image = await _iconManager.GetById(user.IconId.Value);
            var model = new ProfileViewModel
            {
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IconPath = image.Path
            };

            return View(model);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangeProfilePasswordViewModel model)
        {
            if (!ModelState.IsValid)            
                return View(model);
            
            var user = await _userManager.GetUserAsync(User);

            if (user == null)            
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (!changePasswordResult.Succeeded)           
              return BadRequest(changePasswordResult.Errors.ToString());
            

            await _signInManager.SignInAsync(user, isPersistent: false);           
          
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
               return BadRequest();

            var id =await _iconManager.CreateAndGetIconId(model.IconFile);
            var user = await _userManager.GetUserAsync(User);

            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;             
            user.IconId = id;

             var result =  _userManager.UpdateAsync(user);

            if (!result.Result.Succeeded) return BadRequest();

            return RedirectToAction("Index" ,"Profile");
        }
    }
}
