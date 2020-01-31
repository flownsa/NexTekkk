using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

 

namespace Nextekk.Models
{
    [Table("EmployeeRole")]
    public partial class EmployeeRole : IdentityUserRole<string>
    {
        
        // public virtual string EmployeeId { get; set; }  // personal id to track side changes

        public virtual Employee employee { get; set; }
        public virtual Role Role { get; set; }

    }

}

