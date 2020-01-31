using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.Extensions.Options;


// Claims property is Role and UserName and DateOfBirth (DOB)

//  Claims class
namespace Nextekk.Models
{
    // Emulate Claims for principal
    public class EmployeeClaimsPrincipalFactory : UserClaimsPrincipalFactory<Employee, Role>
    {
        // Constructor
        public EmployeeClaimsPrincipalFactory (
            UserManager<Employee> userManager,
            RoleManager<Role> roleManager,
            IOptions<IdentityOptions> optionsAccessor
            ) : base(userManager, roleManager, optionsAccessor) 
            {
                if (userManager == null)
                {
                    throw new ArgumentNullException(nameof(userManager));
                }
                if (RoleManager == null)
                {
                    throw new ArgumentNullException(nameof(RoleManager));
                }
                if (optionsAccessor == null || optionsAccessor.Value == null)
                {
                    throw new ArgumentNullException(nameof(optionsAccessor));
                }
                
            }
            
//      Generate claims as 'uname' from Employee Class - The ClaimPrincipal
        // public override async Task<ClaimsPrincipal> CreateAsync(Employee employee) 
        // {
        //     // IdentityPrincipal to use as claims handler
        //     var principal = await base.CreateAsync(employee);
        //     if (!string.IsNullOrWhiteSpace(employee.FirstName))
        //     {
        //         ((ClaimsIdentity)principal.Identity).AddClaims(
        //             new[] 
        //             {
        //                 new Claim(ClaimTypes.GivenName, employee.FirstName)
        //                 // new Claim(ClaimTypes.UserName, employee.FirstName)

        //             });
        //     }
        //     return principal;
        // }

        //      Generate claims 
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(Employee employee) 
        {
            // call GenerateClaimsAsync from base class as handler for IdentityPrincipal 
            var identity = await base.GenerateClaimsAsync(employee);
            // if (UserManager.SupportsUserRole)
            // {
                // var roles = await UserManager.GetRolesAsync(employee);   // all roles associated with employee
              
            identity.AddClaim(new Claim("Activated", employee.IsActivated ?? ""));   // adds claim type uname to Claims
            identity.AddClaim(new Claim("Admin", employee.IsAdmin?? ""));   // adds claim type uname to Claims

            return identity;

        }
    }
}
