using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Nextekk.Models;

namespace Nextekk.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<Employee> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly Nextekk.Models.NextekkDBContext _db;

        public LoginModel(SignInManager<Employee> signInManager, ILogger<LoginModel> logger, Nextekk.Models.NextekkDBContext db)
        {
            _signInManager = signInManager;
            _logger = logger;
            _db = db;

        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            ReturnUrl = returnUrl;

        }
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");



            // var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            // var claim = claimsIdentity.FindFirst(ClaimTypes.IsActivated);

            if (ModelState.IsValid)
            {   
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.UserName, Input.Password, Input.RememberMe, false);
                

                // var id = _db.Employees.FirstOrDefaultAsync(m => m.UserName == Input.UserName).Id.ToString();  

                if (result.Succeeded)
                {

                var staff = await _db.Employees.FirstOrDefaultAsync(m => m.UserName == Input.UserName);  // Get user data for claims

                _logger.LogInformation("User logged in.");
                    // ~/Identity/Account/Personal?id
                    return Redirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
