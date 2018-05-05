using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CrowdDj.BL.PoCos
{
    public class Party : EntityObject
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        [MaxLength(200)]
        public string Location { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        public ICollection<Guest> Guests { get; set; }

       //public int PlayListId { get; set; }

        //[ForeignKey(nameof(PlayListId))]
        public PlayList PlayList { get; set; }

        public Party()
        {
            Guests = new List<Guest>();
        }
    }
}
