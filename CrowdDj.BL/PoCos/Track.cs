using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CrowdDj.BL.PoCos
{
    public class Track : EntityObject
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        [MaxLength(200)]
        public string Interpret { get; set; }

        [Required]
        [MaxLength(200)]
        public string Url { get; set; }

        public PartyGuest RecommendedByGuest { get; set; }

        public ICollection<Vote> Votes { get; set; }

        public ICollection<PlayList> PlayLists { get; set; }

        public Track()
        {
            Votes = new List<Vote>();
            PlayLists = new List<PlayList>();
        }
    }
}
