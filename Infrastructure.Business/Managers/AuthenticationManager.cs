using AutoMapper;
using Domain.Core.Model;
using Domain.Interfaces;
using Infrastructure.Business.DTOs;
using Infrastructure.Business.Infrastructure;
using Infrastructure.Business.Managers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Business.Services
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;

        public AuthenticationManager(IMapper mapper , IUnitOfWork db)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<OperationDetails> Register(UserDTO userDTO)
        {
            var user = await _db.UserManager.FindByEmailAsync(userDTO.Email);

            if (user == null)
            {
                var userIdentity = _mapper.Map<UserDTO, AppUser>(userDTO);
                var result = await _db.UserManager.CreateAsync(userIdentity, userDTO.Password);

                if (result.Errors.Count() > 0)
                    return new OperationDetails(false, result.Errors.FirstOrDefault().ToString(), "");

                await _db.UserManager.AddToRoleAsync(userIdentity, "User");
                 _db.Save();

                return new OperationDetails(true, "Congratulations! Your account has been created.", "");
            }
            else
            {
                return new OperationDetails(false, "User with this login already exists", "Email");
            }
        }

        public async Task<OperationDetails> Login(UserDTO userDTO)
        {
            var identity = await GetClaimsIdentity(userDTO.UserName, userDTO.Password);
            if (identity == null)
            {
                return new OperationDetails(false, "Invalid username or password.", "");
            }

            var auth = await _db.SignInManager.PasswordSignInAsync(userDTO.UserName, userDTO.Password, userDTO.RememberMe, lockoutOnFailure: false);

            return new OperationDetails(auth.Succeeded, " ", " ");
        }

        public async Task<ConfirmDTO> GetEmailConfirmationToken(string userName)
        {
            var user = await _db.UserManager.FindByNameAsync(userName);
            if (user == null || (await _db.UserManager.IsEmailConfirmedAsync(user)))
                return (null);

            var code = await _db.UserManager.GenerateEmailConfirmationTokenAsync(user);
            return new ConfirmDTO { Code = code, UserId = user.Id };
        }

        public async Task<ConfirmDTO> GetPasswordConfirmationToken(string userName)
        {
            var user = await _db.UserManager.FindByNameAsync(userName);
            if (user == null || !(await _db.UserManager.IsEmailConfirmedAsync(user)))
                return (null);

            var code = await _db.UserManager.GenerateEmailConfirmationTokenAsync(user);
            return new ConfirmDTO { Code = code, UserId = user.Id };
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);

            var userToVerify = await _db.UserManager.FindByNameAsync(userName);
            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

            if (await _db.UserManager.CheckPasswordAsync(userToVerify, password))
                return await Task.FromResult(new ClaimsIdentity());

            return await Task.FromResult<ClaimsIdentity>(null);
        }
        public async Task Logout()
        {
            await _db.SignInManager.SignOutAsync();
        }

        public async Task<OperationDetails> GoogleAuthentication()
        {

            ExternalLoginInfo info = await _db.SignInManager.GetExternalLoginInfoAsync();
            if (info == null) return (null);

            var user = await _db.UserManager.FindByEmailAsync(info.Principal.FindFirst(ClaimTypes.Email).Value);
            if (user != null)
            {
                await _db.SignInManager.SignInAsync(user, false);
                return new OperationDetails(true, "", "");
            }

            var userIdentity = await CreateGoogleUser(info);
            IdentityResult identResult = await _db.UserManager.AddLoginAsync(userIdentity, info);

            if (identResult.Succeeded)
                await _db.SignInManager.SignInAsync(userIdentity, false);

            return new OperationDetails(identResult.Succeeded, "", "");

        }

        private async Task<AppUser> CreateGoogleUser(ExternalLoginInfo info)
        {
            var names = info.Principal.FindFirst(ClaimTypes.Name).Value.Split(' ');
            var userIdentity = new AppUser
            {
                Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                UserName = info.Principal.FindFirst(ClaimTypes.Email).Value,
                EmailConfirmed = true
            };

            IdentityResult identResult = await _db.UserManager.CreateAsync(userIdentity);

			//TODO: Uncomment after Seeding implementation
            //if (identResult.Succeeded)
            //    await _db.UserManager.AddToRoleAsync(userIdentity, "User");

            return userIdentity;
        }

    }
}
