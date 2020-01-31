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
        
        private bool admin = false; // = false;
        public string IsAdmin
        {    
            get
            {
                return admin.ToString();
            }
        }

        public void setAdmin()
        {
            if (admin == true)
            {
                admin = false;
            }
            else
            {
                admin = true;
            }
        }


        private bool isActivated = false; // = false;
        public string IsActivated
        {    
            get
            {
                return isActivated.ToString();
            }
        }

        public void setIsActivated()
        {
            if (isActivated == true)
            {
                isActivated = false;
            }
            else
            {
                isActivated = true;
            }
        }
        
        
        public  Employee()  
        {
            // IdSeed++;        
            // EmployeeId = IdSeed.ToString(); 
                
            admin = false;  
            isActivated = false;
            // EmployeeId = Id;
        }   
        
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        // public string UserName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Dob { get; set; }

        // class Enumerates 
        // { 
        //     public enum Sex{ Female=1, Male=0 } 
        //     public enum MaritalStatus { Single, Married, Divorced, Seperated};
        // }
        AttributeGen.Sex Sex { get; set; }
        AttributeGen.MaritalStatus MaritalStatus { get; set; }
        
        
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

