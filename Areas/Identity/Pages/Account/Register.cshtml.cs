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
        // private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<Employee> userManager,
            SignInManager<Employee> signInManager,
            ILogger<RegisterModel> logger)//,
            //IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
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
                    UserName = Input.Email, 
                    Password = Input.Password,
                    // Sex = Input.Sex;
                    // Sex = Input.MaritalStatus;
                    Dob = DateTime.Parse(Input.Dob).Date,
                    NoOfChildren = int.Parse(Input.NoOfChildren),
                    Email = Input.Email,
                };
                var result = await _userManager.CreateAsync(staff, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Staff created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(staff);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = staff.Id, code = code },
                        protocol: Request.Scheme);

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
