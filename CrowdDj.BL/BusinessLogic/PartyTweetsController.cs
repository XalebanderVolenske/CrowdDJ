using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrowdDj.BL.Contracts;
using CrowdDj.BL.PoCos;

namespace CrowdDj.BL.BusinessLogic
{
    public class PartyTweetsController
    {
        public bool AddPartyTweet(IUnitOfWork unitOfWork, PartyTweet partyTweet)
        {
            if (partyTweet == null || unitOfWork == null)
            {
                throw new ArgumentNullException();
            }

            if (unitOfWork.PartyTweets.Get(pt => pt.Id == partyTweet.Id).Length != 0)
            {
                return false;
            }

            PartyTweet newPartyTweet = new PartyTweet
            {
                Message = partyTweet.Message,
                PartyGuest = partyTweet.PartyGuest
            };
            unitOfWork.PartyTweets.Insert(newPartyTweet);
            unitOfWork.SaveChanges();
            return true;
        }
    }
}
