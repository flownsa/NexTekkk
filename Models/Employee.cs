using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;



namespace Nextekk.Models
{
    [Table("Employee")]
    public partial class Employee : IdentityUser<string>
    {
        // The role name this employee belong to
        // private static int IdSeed = 5000; 
        
        private bool IsAdmin; // = false;
        public bool admin 
        {
            get
            {
                return IsAdmin;
            }
            private set{ setAdmin(); }
        }

        public void setAdmin()
        {

            if (IsAdmin == true)
            {
                IsAdmin = false;
            }
            else
            {
                IsAdmin = false;
            }


        }
        
        
        public  Employee()    // assign default identity id and set default admin value
        {
            // IdSeed++;        
            // EmployeeId = IdSeed.ToString(); 
                
            IsAdmin = false;
            // EmployeeId = Id;
        }   
        
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{yyyy-mm-dd}", ApplyFormatInEditMode = true)]
        public DateTime Dob { get; set; }

        // public enum Sex { Female=1, Male=0 }
        // public enum MaritalStatus { Single, Married, Divorced, Seperated};
        [Required]
        public string Password { get; set; }
        
        [Range(0, 30)]
        public int NoOfChildren { get; set; }
        
//      Navigational properties
        // public virtual ICollection<Claim> Claims{get; set;} // to be instantiated as a list set of IdentityClaim
        // public virtual ICollection<IdentityUserClaim<string>> Claims{get; set;} // to be instantiated as a list set of IdentityClaim
        // public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
        // public virtual ICollection<IdentityUserToken<string>> Tokens { get; set; }
        public virtual OtherInfo OtherInfo { get; set; }  // reference to added user information
        public virtual ICollection<Role> roles { get;}  // Use enumerable to list this employee roles

    }
}

// An operation was scaffolded that may result in the loss of data. Please review the migration for accuracy.

