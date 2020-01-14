using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;


namespace Nextekk.Models
{
    [Table("Role")]
    public partial class Role : IdentityRole<string>
    { 
        public Role () :  base()  // to be used to access IdentityRole in this Role class
        { 
            
            // rolename = Name;  // using identity variables as these
        }
        // public string rolename { get; set; }

        
    }

}
