using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextekk.Models
{

    public static class AttributeGen
    {
        public enum Sex
        { 
            Female=1, 
            Male=0 
        } 
        public enum MaritalStatus 
        { 
            Single = 0, 
            Married = 1, 
            Divorced = 2, 
            Seperated = 3
        };

        public const string NoPermit = "No Permission";
        public const string Admin = "Administrator";
        public const string StaffMember = "Staff Member";
    }
}