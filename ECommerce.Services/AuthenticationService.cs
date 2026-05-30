using AutoMapper;
using ECommerce.Domain.Entities.IdentityModule;
using ECommerce.Services.Abstraction;
using ECommerce.Shared.CommonResposes;
using ECommerce.Shared.IdentityDTOs;
using ECommerce.Shared.OrdersDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMapper _mapper;

        public AuthenticationService(UserManager<ApplicatonUser> userManager , IConfiguration configuration , IMapper mapper)
        {
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<bool> EmailExistAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<Result<AddressDTO>> GetUserAddressAsync(string email)
        {
            var user = await _userManager.Users.Include(X => X.Address).FirstOrDefaultAsync(X => X.Email == email);

            if (user is null)
                return Error.NotFound("User.Notfound", $"User Not found With This Email:{email}");

            var AddressDto = _mapper.Map<AddressDTO>(user.Address);

            return AddressDto;

        }
      
        public async Task<Result<AddressDTO>> UpdatedUserAddress(AddressDTO addressDTO, string email)
        {
            var user = await _userManager.Users.Include(X => X.Address).FirstOrDefaultAsync(X => X.Email == email);

            if (user is null)
                return Error.NotFound("User.Notfound", $"User Not found With This Email:{email}");

            if(user.Address is not null) // Update
            {
                user.Address.FirstName = addressDTO.FirstName;
                user.Address.LastName = addressDTO.LastName;
                user.Address.City = addressDTO.City;
                user.Address.Street = addressDTO.Street;
                user.Address.Country = addressDTO.Country;
            }
            else // Create
            {
                user.Address = _mapper.Map<Address>(addressDTO);
            }

            await _userManager.UpdateAsync(user);

            return _mapper.Map<AddressDTO>(user.Address);
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
                PhoneNumber = registerDTO.PhoneNumber,
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
