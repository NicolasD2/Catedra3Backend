using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PostCatedraApi.src.Models;

namespace PostCatedraApi.src.Interfaces
{
    public interface IUserRepository
    {
        Task<IdentityResult> CreateAsync(Usuario usuario, string Password);
        Task<Usuario> FindByEmailAsync(string Email);
        Task<SignInResult> PasswordSignInAsync(string Email, string password, bool isPersistent, bool lockoutOnfailure);
    }
}