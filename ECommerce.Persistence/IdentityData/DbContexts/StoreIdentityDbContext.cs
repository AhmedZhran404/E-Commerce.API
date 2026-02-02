using ECommerce.Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.IdentityData.DbContexts
{
    public class StoreIdentityDbContext : IdentityDbContext<ApplicatonUser>
    {
        public StoreIdentityDbContext(DbContextOptions<StoreIdentityDbContext> options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Address>().ToTable("Addresses");
           
            builder.Entity<ApplicatonUser>().ToTable("Users");
          
            builder.Entity<IdentityRole>().ToTable("Roles");
       
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
           
        }
        
    }
}
