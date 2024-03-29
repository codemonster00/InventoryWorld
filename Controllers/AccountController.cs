using InventoryWorld.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace InventoryWorld.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private AppDbContext _context;
        private readonly ILogger _logger;
        private readonly RoleManager<AppRole>_roleManager;


        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, AppDbContext context, RoleManager<AppRole> roleManager)
        {
            _signInManager = signInManager; 
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            
                
        }
        [AllowAnonymous]
        [HttpGet]

        public async Task<IActionResult> Login(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {

                var user = await _userManager.FindByEmailAsync(model.Email);
                if(user is not null)
                {
                    if (user.UserName == "" || user.UserName == null)
                    {
                        ModelState.AddModelError("Error", "User currently Disabled");
                        return View();
                    }
                }

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                  //  _logger.LogInformation("User logged in.");
                    return RedirectToLocal(returnUrl);
                }

                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            return View(model);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
           // _logger.LogInformation("User logged out.");


            return RedirectToAction(nameof(AccountController.Login), "Account");
        }

        public IActionResult AddUser()
        {

            ViewData["Role"] = new SelectList(_context.Roles, "Name", "Name", "Please Select Role");
            ViewBag.Roles = _context.Roles;


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(Register register)
        {

            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {

                    FirstName = register.FirstName,
                    LastName = register.LastName,
                    UserName = register.Email,
                    Email = register.Email,
                    NormalizedEmail= register.Email.ToUpper()
                };
                var result = await _userManager.CreateAsync(user, register.Password);
                if (result.Succeeded)
                {
                  //  _logger.LogInformation("User created a new account with password.");
                    if (await _roleManager.RoleExistsAsync(register.Role))
                    {
                        await _userManager.AddToRoleAsync(user, register.Role);

                    }
                  //  _logger.LogInformation("Role assigned to user.");
                    return RedirectToAction("AllUsers", "Account");

                }



            }
            return RedirectToAction("AllUsers", "Account");
        }

        public IActionResult AllUsers()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        public async Task<IActionResult> Disable(string id) {


          var user= await _userManager.FindByIdAsync(id);
            if (user != null)
            {
               
                
                user.UserName = "";

                _context.SaveChanges();

                
               // var tern = _userManager.IsLockedOutAsync(user);
            }
            return RedirectToAction("AllUsers");
        }


         public async Task<IActionResult> Enable(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user != null)
            {
                if(user.Email!= null)
                {
                    user.UserName=user.Email;
                    _context.SaveChanges();
                    return RedirectToAction("AllUsers");
                }
            
            }
            return RedirectToAction("AllUsers");
        }

    }
}
