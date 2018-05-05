using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrowdDj.BL.PoCos
{
    public class PlayList : EntityObject
    {
        [Required]
        //[ForeignKey(nameof(PartyId))]
        public Party Party { get; set; }

        //public int PartyId { get; set; }

        public ICollection<Vote> Votes { get; set; }

        public ICollection<Track> Tracks { get; set; }

        public PlayList()
        {
            Votes = new List<Vote>();
            Tracks = new List<Track>();
        }
    }
}
