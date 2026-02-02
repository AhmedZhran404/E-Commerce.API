using ECommerce.Shared.CommonResposes;
using ECommerce.Shared.IdentityDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Abstraction
{
    public interface IAuthenticationService
    {
        // Login -> (Email , Password) return --> [Token , DisplayName , Email]
        Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO);
        // Register --> (Email , Pass , DisplayName , UserName , Phone) , return --> [Token , DisplayName , Email] 
        Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO);


        // Email => bool
        Task<bool> EmailExistAsync(string email);

        // Email => Result of UserDTO [Email - DisplayName - Token]
        // If Found User in Database , If Not Found Return [NotFoundError]
        Task<Result<UserDTO>> GetUserByEmailAsync(string email);
    }
}
