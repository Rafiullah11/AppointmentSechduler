using AppointmentSechduler.Data;
using AppointmentSechduler.Models;
using AppointmentSechduler.Models.ViewModels;
using AppointmentSechduler.Utlities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AppointmentSechduler.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _db;
         SignInManager<ApplicationUser> _signInManager;
         UserManager<ApplicationUser> _userManager;
         RoleManager<IdentityRole> _roleManager;

        public AccountController(AppDbContext db,SignInManager<ApplicationUser> signInManager,
                UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Login()
        {
            return  View();
        }

        [HttpPost]
        //[AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.Email);
                    HttpContext.Session.SetString("ssuserName", user.Name);
                    return RedirectToAction("Index", "Appointment");
                }
               
            }

            ModelState.AddModelError("", "Invalid attempt");
            return View(model);
        } 

        [HttpGet]
        public async Task<IActionResult> Register()
        {
          
            return  View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register( RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.Name
                };
                var result =await _userManager.CreateAsync(user,model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.RoleName);
                    if (!User.IsInRole(Helper.Admin))
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                    }
                    TempData["newAdminSignUp"] = user.Name;
                    return RedirectToAction("Index","Appointment");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login","Account");
        }
    }
}
