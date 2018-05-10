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
    public class PartiesControllerTests
    {
        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void AddParty_UnitOfWorkNull_ShouldThrowException()
        {
            PartiesController partiesController = new PartiesController();
            Guest guest = new Guest { EmailAddress = "email@dummy.at" };
            Party party = new Party
            {
                Guests = new List<Guest> { guest },
                EndTime = DateTime.Now,
                StartTime = DateTime.Now
            };
            PlayList playList = new PlayList { Party = party };
            Track track = new Track { Title = "Song1", Interpret = "Interpret", PlayLists = new List<PlayList> { playList } };
            partiesController.AddParty(null, party);
        }

        [TestMethod]
        public void AddParty_NewCorrectParty_ShouldAddInDatabase()
        {
            IUnitOfWork unitOfWork = new MockUnitOfWork();
            PartiesController partiesController = new PartiesController();
            Guest guest = new Guest { EmailAddress = "email@dummy.at" };
            Party party = new Party
            {
                Guests = new List<Guest> { guest },
                Name = "Party1",
                Location = "Beim Pepi",
                EndTime = DateTime.Now,
                StartTime = DateTime.Now
            };
            PlayList playList = new PlayList { Party = party };
            Track track = new Track { Title = "Song1", Interpret = "Interpret", PlayLists = new List<PlayList> { playList } };
            unitOfWork.Guests.Insert(guest);
            unitOfWork.PlayLists.Insert(playList);
            unitOfWork.Tracks.Insert(track);
            bool ok = partiesController.AddParty(unitOfWork, party);
            Assert.IsTrue(ok);
            var parties = unitOfWork.Parties.Get();
            Assert.IsTrue(parties.Any(p=>p.Name == party.Name 
                                         && p.Location == party.Location 
                                         && p.StartTime == party.StartTime 
                                         && p.EndTime == party.EndTime));
        }
    }
}
