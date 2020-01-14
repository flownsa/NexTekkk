using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Nextekk.Models
{
    [Table("OtherInfo")]
    public partial class OtherInfo
    {
        [Key]
        public string Id { get; set; }
        public bool Activated { get; set; }
        public DateTime? DateJoined { get; set; }
        public string MaxEdu { get; set; }
        public string School { get; set; }
        public DateTime? TimeServed { get; set; }
        public string Position { get; set; }
        public DateTime? DatePromoted { get; set; }
        public double? Salary { get; set; }
        // public virtual Employee Employee { get; set; }
    }
}
