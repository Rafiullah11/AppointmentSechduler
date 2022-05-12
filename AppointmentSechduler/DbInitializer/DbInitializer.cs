using AppointmentSechduler.Data;
using AppointmentSechduler.Models;
using AppointmentSechduler.Utlities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace AppointmentSechduler.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public DbInitializer(AppDbContext db, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception )
            {

            }

            if (_db.Roles.Any(x => x.Name == Utlities.Helper.Admin)) return;


            _roleManager.CreateAsync(new IdentityRole(Helper.Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Helper.Doctor)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Helper.Patient)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                Name = "Admin Rafi"

            }, "Admin@11").GetAwaiter().GetResult();

            ApplicationUser user = _db.Users.FirstOrDefault(u => u.Email == "admin@gmail.com");
            _userManager.AddToRoleAsync(user, Helper.Admin).GetAwaiter().GetResult();

        }
    }
}