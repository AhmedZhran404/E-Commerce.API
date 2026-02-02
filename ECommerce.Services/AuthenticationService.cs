using ECommerce.Domain.Entities.IdentityModule;
using ECommerce.Services.Abstraction;
using ECommerce.Shared.CommonResposes;
using ECommerce.Shared.IdentityDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicatonUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthenticationService(UserManager<ApplicatonUser> userManager , IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<bool> EmailExistAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<Result<UserDTO>> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if(user is null)
                return Error.NotFound("User.NotFound" , $"User With This Email:{email} Is Not Found");

            return new UserDTO(user.Email!, user.DisplayName, await CreateTokenAsync(user));

        }

        public async Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO)
        {

           var user = await _userManager.FindByEmailAsync(loginDTO.Email);

            if (user is null)
                return Error.InvalidCredintals("User.InvalidCredintial");

            var IsPasswordVail = await _userManager.CheckPasswordAsync(user , loginDTO.Password);


            if(!IsPasswordVail)
                return Error.InvalidCredintals("User.InvalidCredintial");

            var token = await CreateTokenAsync(user);

            return new UserDTO(user.Email!, user.DisplayName, token);

        }

        public async Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO)
        {
            var user = new ApplicatonUser()
            {
                Email = registerDTO.Email,
                DisplayName = registerDTO.DisplayName,
                PhoneNumber = registerDTO.Phone,
                UserName = registerDTO.UserName,
            };

            var identityResult =  await _userManager.CreateAsync(user , registerDTO.Password);

            if (identityResult.Succeeded)
            {
                var token = await CreateTokenAsync(user);
                return new UserDTO(user.Email, user.UserName, token);
            }

            return identityResult.Errors.Select(E => Error.Validation(E.Code, E.Description)).ToList();


        }
   
    
        private async Task<string> CreateTokenAsync(ApplicatonUser User)
        {

            var claim = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email , User.Email!),
                new Claim(JwtRegisteredClaimNames.Name , User.UserName!)
            };

            var roles = await _userManager.GetRolesAsync(User);

            foreach (var role in roles)
            {
                claim.Add(new Claim(ClaimTypes.Role , role));
            }

            var secretKey = _configuration["JWTOptions:SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            var cred = new SigningCredentials(key , SecurityAlgorithms.HmacSha256);


             var token = new JwtSecurityToken
             (
                    issuer: _configuration["JWTOptions:Issuer"],
                    audience: _configuration["JWTOptions:Audience"],
                    claims: claim,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: cred
             );


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
