using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Nextekk.Models;

namespace Nextekk.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<Employee> _signInManager;
        private readonly UserManager<Employee> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly RoleManager<Role> _roleManager;
        
        // private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<Employee> userManager,
            SignInManager<Employee> signInManager,
            ILogger<RegisterModel> logger//,
            //IEmailSender emailSender),
            ,RoleManager<Role> roleManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            // _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {   
            [Required]
            [StringLength(50)]
            [Display(Name = "FirstName")]
            public string FirstName { get; set; }

            [Required]
            [StringLength(50)]
            [Display(Name = "LastName")]
            public string LastName { get; set; }

            [Required]
            [StringLength(50)]
            [Display(Name = "UserName")]
            public string UserName { get; set; }

            [Required]

            [Display(Name = "Number of Children")]
            public string NoOfChildren { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}", ApplyFormatInEditMode = true)]
            [Display(Name = "Date Of Birth")]
            public string Dob { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var staff = new Employee 
                { 
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    UserName = Input.UserName,
                    Email = Input.Email, 
                    Password = Input.Password,
                    EmailConfirmed = false,
                    // Sex = Input.Sex;  // radio buttons
                    // Sex = Input.MaritalStatus;  //radio buttons 
                    Dob = DateTime.Parse(Input.Dob).Date, 
                    NoOfChildren = int.Parse(Input.NoOfChildren)                    
                };
                
                var result = await _userManager.CreateAsync(staff, Input.Password);
                if (result.Succeeded)
                // Add roles by default
                {
                    if ( !await _roleManager.RoleExistsAsync(AttributeGen.Admin))
                    {
                        await _roleManager.CreateAsync(new Role(AttributeGen.Admin));
                    } 
                    
                    if ( !await _roleManager.RoleExistsAsync(AttributeGen.StaffMember))
                    {
                        await _roleManager.CreateAsync(new Role(AttributeGen.StaffMember));
                    }

                    if ( !await _roleManager.RoleExistsAsync(AttributeGen.NoPermit))
                    {
                        await _roleManager.CreateAsync(new Role(AttributeGen.NoPermit));
                    }
                    await _userManager.AddToRoleAsync(staff, AttributeGen.NoPermit); // default permission for new user
                    _logger.LogInformation("Staff created a new account with password.");

                    // var code = await _userManager.GenerateEmailConfirmationTokenAsync(staff);
                    // var callbackUrl = Url.Page(
                    //     "/Account/ConfirmEmail",
                    //     pageHandler: null,
                    //     values: new { userId = staff.Id, code = code },
                    //     protocol: Request.Scheme);

                    // await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        // $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await _signInManager.SignInAsync(staff, isPersistent: false);
                    return LocalRedirect(returnUrl);
                    // return Redirect(returnUrl);

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
