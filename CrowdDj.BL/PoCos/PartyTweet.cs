using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrowdDj.BL.PoCos
{
    public class PartyTweet : EntityObject
    {
        [Required]
        [MaxLength(300)]
        public String Message { get; set; }

        [Required]
        public PartyGuest PartyGuest { get; set; }
    }
}
