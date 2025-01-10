using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PostCatedraApi.src.Interfaces;
using PostCatedraApi.src.Models;

namespace PostCatedraApi.src.Repository
{
    public class UserRepository: IUserRepository
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario>_signInManager;

        public UserRepository(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager){
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> CreateAsync(Usuario usuario, string password){
            return await _userManager.CreateAsync(usuario, password);
        }

        public async Task<Usuario> FindByEmailAsync(string Email){
            return await _userManager.FindByEmailAsync(Email);
        }
        public async Task<SignInResult> PasswordSignInAsync(string Email, string password, bool isPersistent, bool lockoutOnfailure){
            return await _signInManager.PasswordSignInAsync(Email,password,isPersistent,lockoutOnfailure);
        }
    }

}