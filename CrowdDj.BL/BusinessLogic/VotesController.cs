using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrowdDj.BL.Contracts;
using CrowdDj.BL.PoCos;

namespace CrowdDj.BL.BusinessLogic
{
    public class VotesController
    {

        public bool AddVote(IUnitOfWork unitOfWork, Guest guest, PlayList playList, Track track)
        {

            if (guest == null || playList == null || track == null || unitOfWork == null)
            {
                throw new ArgumentNullException();
            }
            if (unitOfWork.Votes.Get(v =>
                    v.GuestId == guest.Id && v.PlayListId == playList.Id && v.TrackId == track.Id).Length != 0)
            {
                return false;
            }
            
            Vote newVote = new Vote
            {
                Guest = guest,
                PlayList = playList,
                Track = track,
                TimeStamp = DateTime.Now
            };
            unitOfWork.Votes.Insert(newVote);
            unitOfWork.SaveChanges();
            return true;
        }
    }
}
