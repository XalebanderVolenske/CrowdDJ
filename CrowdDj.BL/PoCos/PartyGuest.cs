using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrowdDj.BL.PoCos
{
    public class PartyGuest : EntityObject
    {
        public int PartyCode { get; set; }

        [ForeignKey(nameof(GuestId))]
        public Guest Guest { get; set; }

        public int GuestId { get; set; }

        [ForeignKey(nameof(PartyId))]
        public Party Party { get; set; }

        public int PartyId { get; set; }

        public ICollection<PartyTweet> PartyTweets { get; set; }
    }
}
