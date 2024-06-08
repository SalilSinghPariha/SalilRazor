using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Salil.Model;
using Salil.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salil.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDBContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(
            UserManager<IdentityUser> userManager,
            ApplicationDBContext dbContext,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            try 
            {
                if (_dbContext.Database.GetPendingMigrations().Count() > 0)
                {
                    _dbContext.Database.Migrate();
                }

            }
            catch(Exception)
            { 
            }

            if (!_roleManager.RoleExistsAsync(SD.kitchenRole).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.kitchenRole)).GetAwaiter().GetResult();
              
                _roleManager.CreateAsync(new IdentityRole(SD.managerRole)).GetAwaiter().GetResult();
            
                _roleManager.CreateAsync(new IdentityRole(SD.frontDeskRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.customerRole)).GetAwaiter().GetResult();

                // now after this create application user for first time migration to Azure for SQL database


                _userManager.CreateAsync(
                    new ApplicationUser{
                        UserName= "********************",
                        Email = "*********************",
                        EmailConfirmed = true,
                        firstName = "salil",
                        lastName = "singh"
                    }, "*******").GetAwaiter().GetResult();

                ApplicationUser applicationUser= _dbContext.applicationUsers.FirstOrDefault(u=>
                u.Email=="***********");

                _userManager.AddToRoleAsync(applicationUser, SD.managerRole).GetAwaiter().GetResult();
            }

            return;

        }
    }
}
