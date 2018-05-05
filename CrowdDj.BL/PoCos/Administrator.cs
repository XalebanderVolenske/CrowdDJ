using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace CrowdDj.BL.PoCos
{
    [Table("Administrators")]
    public class Administrator : User
    {
        [Required]
        [MaxLength(200)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(200)]
        public string Password { get; set; }
    }
}
