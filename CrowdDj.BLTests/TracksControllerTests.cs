using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrowdDj.BL.Contracts;
using CrowdDj.BL.BusinessLogic;
using CrowdDj.BL.PoCos;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CrowdDj.BLTests
{
    [TestClass]
    public class TracksControllerTests
    {
        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void AddTrack_UnitOfWorkNull_ShouldThrowException()
        {
            TracksController tracksController = new TracksController();
            Guest guest = new Guest { EmailAddress = "email@dummy.at" };
            Party party = new Party
            {
                Guests = new List<Guest> { guest },
                EndTime = DateTime.Now,
                StartTime = DateTime.Now,
                PlayList =  new PlayList()
            };
            PartyGuest partyGuest = new PartyGuest {Party = party, Guest = guest, PartyCode = 1};
            PlayList playList = new PlayList { Party = party };
            Track track = new Track { Title = "Song1", Interpret = "Interpret", PlayLists = new List<PlayList> { playList }, RecommendedByGuest = partyGuest};
            tracksController.AddTrack(null, track);
        }

        [TestMethod]
        public void AddTrack_NewCorrectTrack_ShouldAddInDatabase()
        {
            IUnitOfWork unitOfWork = new MockUnitOfWork();
            TracksController tracksController = new TracksController();
            Guest guest = new Guest { EmailAddress = "email@dummy.at" };
            Party party = new Party
            {
                Guests = new List<Guest> { guest },
                EndTime = DateTime.Now,
                StartTime = DateTime.Now,
                PlayList = new PlayList()
            };
            PartyGuest partyGuest = new PartyGuest { Party = party, Guest = guest, PartyCode = 1 };
            PlayList playList = new PlayList { Party = party };
            Track track = new Track { Title = "Song1", Interpret = "Interpret", PlayLists = new List<PlayList> { playList }, RecommendedByGuest = partyGuest };
            unitOfWork.Parties.Insert(party);
            unitOfWork.PlayLists.Insert(playList);
            unitOfWork.PartyGuests.Insert(partyGuest);
            bool ok = tracksController.AddTrack(unitOfWork, track);
            Assert.IsTrue(ok);
            var tracks = unitOfWork.Tracks.Get();
            Assert.IsTrue(tracks.Any(t => t.Title == track.Title 
                                          && t.Interpret == track.Interpret 
                                          && t.Url == track.Url
                                          && t.PlayLists.Equals(track.PlayLists)
                                          && t.RecommendedByGuest == track.RecommendedByGuest));
        }
    }
}
