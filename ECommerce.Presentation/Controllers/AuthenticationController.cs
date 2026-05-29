using ECommerce.Services.Abstraction;
using ECommerce.Shared.CommonResposes;
using ECommerce.Shared.IdentityDTOs;
using ECommerce.Shared.OrdersDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{
    // Authentication/address
    // Authentication
    public class AuthenticationController : ApiBaseControllers
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        // Login
        // POST:BaseUrl/Api/Authentication/Login
        [HttpPost("Login")]
        public  async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var result = await _authenticationService.LoginAsync(loginDTO);

            return HandleResult<UserDTO>(result);
        }

        [HttpPost("Register")]
        public  async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            var result = await _authenticationService.RegisterAsync(registerDTO);

            return HandleResult<UserDTO>(result);
        }

        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>> CheckEmail(string email)
        {
            var result = await _authenticationService.EmailExistAsync(email);

            return Ok(result);
        }

        [Authorize]
        [HttpGet("CurrentUser")]
        public  async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var Result = await _authenticationService.GetUserByEmailAsync(GetEmailFromToken());

            return HandleResult(Result);
        }

        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDTO>> GetUserAddress()
        {
            var result = await _authenticationService.GetUserAddressAsync(GetEmailFromToken());

            return HandleResult(result);
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDTO>> GetUserAddress()
        {
            var result = await _authenticationService.GetUserAddressAsync(GetEmailFromToken());

            return HandleResult(result);
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDTO>> UpdateUserAddress(AddressDTO addressDTO)
        {
            var result = await _authenticationService.UpdatedUserAddress(addressDTO , GetEmailFromToken());

            return HandleResult(result);
        }



    }
}
