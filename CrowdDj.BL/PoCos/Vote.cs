using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrowdDj.BL.PoCos
{
    public class Vote : EntityObject
    {
        [Required]
        public DateTime TimeStamp { get; set; }

        [ForeignKey(nameof(GuestId))]
        public Guest Guest { get; set; }

        [Required]
        public int GuestId { get; set; }
      
        [ForeignKey(nameof(TrackId))]
        public Track Track { get; set; }

        [Required]
        public int TrackId { get; set; }

        public int PlayListId { get; set; }

        [Required]
        [ForeignKey(nameof(PlayListId))]
        public PlayList PlayList { get; set; }

    }
}
