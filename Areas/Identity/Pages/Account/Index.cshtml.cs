// using System;
// using System.Collections.Generic;
// using System.ComponentModel.DataAnnotations;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Authentication;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.RazorPages;
// using Microsoft.Extensions.Logging;
// using Nextekk.Models;

// namespace Nextekk.Areas.Identity.Pages.Account
// {
//     [AllowAnonymous]
//     public class IndexModel : PageModel
//     {
//         private readonly UserManager<Employee> _userManager;
//         private readonly SignInManager<Employee> _signInManager;
//         // private readonly IEmailSender _emailSender;
//         private readonly ILogger<Employee> _logger;


//         public IndexModel(
//             UserManager<Employee> userManager, 
//             SignInManager<Employee> signInManager,
//             /*IEmailSender emailSender */
//             ILogger<LoginModel> logger)
//         {
//             _userManager = userManager;
//             _signInManager = signInManager;
//             //_emailSender = emailSender;
//         }

//         [BindProperty]
//         public InputModel Input { get; set; }
//         public string IsEmailconfirmed { get; set; }

//         [TempData]
//         public string StatusMessage { get; set; }

//         public class InputModel
//         {
//             [Required]
//             public string UserName { get; set; }

//             [Required]
//             [DataType(DataType.Password)]
//             public string Password { get; set; }

//             [Display(Name = "Remember me?")]
//             public bool RememberMe { get; set; }
//         }

//         public void OnGet(string returnUrl = null)
//         {
//             if (!string.IsNullOrEmpty(ErrorMessage))
//             {
//                 ModelState.AddModelError(string.Empty, ErrorMessage);
//             }

//             ReturnUrl = returnUrl;

//         }

//         public async Task<IActionResult> OnPostAsync(string returnUrl = null)
//         {
//             returnUrl = returnUrl ?? Url.Content("~/");

//             if (ModelState.IsValid)
//             {   
//                 // This doesn't count login failures towards account lockout
//                 // To enable password failures to trigger account lockout, set lockoutOnFailure: true
//                 var result = await _signInManager.PasswordSignInAsync(Input.UserName, Input.Password, Input.RememberMe, false);
//                 if (result.Succeeded)
//                 {
//                     _logger.LogInformation("User logged in.");
//                     return Redirect(returnUrl);
//                 }
//                 if (result.RequiresTwoFactor)
//                 {
//                     return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
//                 }
//                 if (result.IsLockedOut)
//                 {
//                     _logger.LogWarning("User account locked out.");
//                     return RedirectToPage("./Lockout");
//                 }
//                 else
//                 {
//                     ModelState.AddModelError(string.Empty, "Invalid login attempt.");
//                     return Page();
//                 }
//             }

//             // If we got this far, something failed, redisplay form
//             return Page();
//         }
//     }
// }
