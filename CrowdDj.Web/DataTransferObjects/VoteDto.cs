using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrowdDj.Web.DataTransferObjects
{
    public class VoteDto
    {
        public int PlayListId { get; set; }

        public int GuestId { get; set; }

        public int TrackId { get; set; }
    }
}
