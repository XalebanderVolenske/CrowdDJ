using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CrowdDj.BL.PoCos
{
    public abstract class User : EntityObject
    {
        [Required]
        [MaxLength(200)]
        public string EmailAddress { get; set; }
    }
}
