using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.IdentityModule
{
    public class ApplicatonUser : IdentityUser
    {
        public string DisplayName { get; set; } = default!;

        public Address Address { get; set; }

    }
}
