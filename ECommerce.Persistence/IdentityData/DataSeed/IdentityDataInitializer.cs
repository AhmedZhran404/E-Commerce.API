using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.IdentityData.DataSeed
{
    public class IdentityDataInitializer : IDataInitializer
    {
        private readonly UserManager<ApplicatonUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<IdentityDataInitializer> _logger;

        public IdentityDataInitializer(UserManager<ApplicatonUser> userManager 
                                    , RoleManager<IdentityRole> roleManager
                                    , ILogger<IdentityDataInitializer> logger
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {

            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }

                if (!_userManager.Users.Any())
                {
                    var user01 = new ApplicatonUser()
                    {
                        DisplayName = "Omar Ahmed",
                        UserName = "OmarAhmed",
                        Email = "OmarAhmed@gmail.com",
                        PhoneNumber = "01234567999"
                    };
                    var user02 = new ApplicatonUser()
                    {
                        DisplayName = "Farida Ahmed",
                        UserName = "FaridaAhmed",
                        Email = "FaridaAhmed@gmail.com",
                        PhoneNumber = "01234567888"

                    };

                   await _userManager.CreateAsync(user01 , "P@ssw0rd");
                   await _userManager.CreateAsync(user02 , "P@ssw0rd");
                         
                   await _userManager.AddToRoleAsync(user01 , "SuperAdmin");
                   await _userManager.AddToRoleAsync(user02 , "Admin");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error While Seeding DataBase, {ex.Message} Happened");
            }
            
        }
    }
}
