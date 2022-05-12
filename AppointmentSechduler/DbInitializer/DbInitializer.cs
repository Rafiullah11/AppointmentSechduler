using AppointmentSechduler.Data;
using AppointmentSechduler.Models;
using AppointmentSechduler.Utlities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AppointmentSechduler.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly AppDbContext _db;
        SignInManager<ApplicationUser> _signInManager;
        UserManager<ApplicationUser> _userManager;
        RoleManager<IdentityRole> _roleManager;

        public DbInitializer(AppDbContext db, SignInManager<ApplicationUser> signInManager,
                UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count()>0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (System.Exception)
            {

                if (_db.Roles.Any(x => x.Name == Helper.Admin)) return;

                     _roleManager.CreateAsync(new IdentityRole(Helper.Admin)).GetAwaiter().GetResult();
                     _roleManager.CreateAsync(new IdentityRole(Helper.Doctor)).GetAwaiter().GetResult();
                     _roleManager.CreateAsync(new IdentityRole(Helper.Patient)).GetAwaiter().GetResult();
                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin@gmail.com",
                    Email ="admin@gmail.com",
                    EmailConfirmed = true,
                    Name="Admin Rafi",
                }, "Admin@11").GetAwaiter().GetResult();

                ApplicationUser user = _db.Users.FirstOrDefault(x => x.Email == "admin@gmail.com");
                _userManager.AddToRoleAsync(user,Helper.Admin).GetAwaiter().GetResult();
            }
        }
    }
}
