using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrowdDj.BL.BusinessLogic;
using CrowdDj.BL.Contracts;
using CrowdDj.BL.PoCos;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CrowdDj.BLTests
{
    [TestClass]
    public class PartyTweetsControllerTests
    {
        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void AddPartyTweet_UnitOfWorkNull_ShouldThrowException()
        {
            PartyTweetsController partyTweetsController = new PartyTweetsController();
            Guest guest = new Guest { EmailAddress = "email@dummy.at" };
            Party party = new Party
            {
                Guests = new List<Guest> { guest },
                EndTime = DateTime.Now,
                StartTime = DateTime.Now
            };
            PartyGuest partyGuest = new PartyGuest{Guest = guest, Party = party};
            PartyTweet partyTweet = new PartyTweet {Message = "TestMessage", PartyGuest = partyGuest};
            partyTweetsController.AddPartyTweet(null, partyTweet);
        }

        [TestMethod]
        public void AddPartyTweet_NewCorrectPartyTweet_ShouldAddInDatabase()
        {
            IUnitOfWork unitOfWork = new MockUnitOfWork();
            PartyTweetsController partyTweetsController = new PartyTweetsController();
            Guest guest = new Guest { EmailAddress = "email@dummy.at" };
            Party party = new Party
            {
                Guests = new List<Guest> { guest },
                EndTime = DateTime.Now,
                StartTime = DateTime.Now
            };
            PartyGuest partyGuest = new PartyGuest { Guest = guest, Party = party };
            PartyTweet partyTweet = new PartyTweet { Message = "TestMessage", PartyGuest = partyGuest };
            unitOfWork.Parties.Insert(party);
            unitOfWork.Guests.Insert(guest);
            unitOfWork.PartyGuests.Insert(partyGuest);
            bool ok = partyTweetsController.AddPartyTweet(unitOfWork, partyTweet);
            Assert.IsTrue(ok);
            var partyTweets = unitOfWork.PartyTweets.Get();
            Assert.IsTrue(partyTweets.Any(pt => pt.Message == partyTweet.Message && pt.PartyGuest == partyGuest));
        }
    }
}
