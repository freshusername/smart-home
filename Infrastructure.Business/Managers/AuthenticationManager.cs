using AutoMapper;
using Domain.Core.Model;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Infrastructure.Business.DTOs;
using Infrastructure.Business.Infrastructure;
using Infrastructure.Business.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
//TODO: Replace manager Interfaces and Implementations in defferent folders
namespace Infrastructure.Business.Interfaces
{
    public class AuthenticationManager : BaseManager, IAuthenticationManager
    {
        public AuthenticationManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<OperationDetails> Register(UserDTO userDTO)
        {
            var user = await unitOfWork.UserManager.FindByEmailAsync(userDTO.Email);

            if (user == null)
            {
                var userIdentity = mapper.Map<UserDTO, AppUser>(userDTO);
                var result = await unitOfWork.UserManager.CreateAsync(userIdentity, userDTO.Password);

                if (result.Errors.Count() > 0)
                    return new OperationDetails(false, result.Errors.FirstOrDefault().ToString(), "");

                await unitOfWork.UserManager.AddToRoleAsync(userIdentity, "User");
                unitOfWork.Save();

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

            var auth = await unitOfWork.SignInManager.PasswordSignInAsync(userDTO.UserName, userDTO.Password, userDTO.RememberMe, lockoutOnFailure: false);

            return new OperationDetails(auth.Succeeded, " ", " ");
        }

        public async Task<ConfirmDto> GetEmailConfirmationToken(string userName)
        {
            var user = await unitOfWork.UserManager.FindByNameAsync(userName);
            if (user == null || (await unitOfWork.UserManager.IsEmailConfirmedAsync(user)))
                return (null);

            var code = await unitOfWork.UserManager.GenerateEmailConfirmationTokenAsync(user);
            return new ConfirmDto { Code = code, UserId = user.Id };
        }

        public async Task<ConfirmDto> GetPasswordConfirmationToken(string userName)
        {
            var user = await unitOfWork.UserManager.FindByNameAsync(userName);
            if (user == null || !(await unitOfWork.UserManager.IsEmailConfirmedAsync(user)))
                return (null);

            var code = await unitOfWork.UserManager.GenerateEmailConfirmationTokenAsync(user);
            return new ConfirmDto { Code = code, UserId = user.Id };
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);

            var userToVerify = await unitOfWork.UserManager.FindByNameAsync(userName);
            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

            if (await unitOfWork.UserManager.CheckPasswordAsync(userToVerify, password))
                return await Task.FromResult(new ClaimsIdentity());

            return await Task.FromResult<ClaimsIdentity>(null);
        }
        public async Task Logout()
        {
            await unitOfWork.SignInManager.SignOutAsync();
        }

        public async Task<OperationDetails> GoogleAuthentication()
        {

            ExternalLoginInfo info = await unitOfWork.SignInManager.GetExternalLoginInfoAsync();
            if (info == null) return (null);

            var user = await unitOfWork.UserManager.FindByEmailAsync(info.Principal.FindFirst(ClaimTypes.Email).Value);
            if (user != null)
            {
                await unitOfWork.SignInManager.SignInAsync(user, false);
                return new OperationDetails(true, "", "");
            }

            var userIdentity = await CreateGoogleUser(info);
            IdentityResult identResult = await unitOfWork.UserManager.AddLoginAsync(userIdentity, info);

            if (identResult.Succeeded)
                await unitOfWork.SignInManager.SignInAsync(userIdentity, false);

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

            IdentityResult identResult = await unitOfWork.UserManager.CreateAsync(userIdentity);

            if (identResult.Succeeded)
                await unitOfWork.UserManager.AddToRoleAsync(userIdentity, "User");

            return userIdentity;
        }

    }
}
