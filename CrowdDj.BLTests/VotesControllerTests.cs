using System;
using System.Collections.Generic;
using System.Linq;
using CrowdDj.BL.BusinessLogic;
using CrowdDj.BL.Contracts;
using CrowdDj.BL.PoCos;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CrowdDj.BLTests
{
    [TestClass]
    public class VotesControllerTests
    {
        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void AddVote_UnitOfWorkNull_ShouldThrowException()
        {
            VotesController votesController = new VotesController();
            Guest guest = new Guest{EmailAddress = "email@dummy.at"};
            Party party = new Party
            {
                Guests = new List<Guest> {guest},
                EndTime = DateTime.Now,
                StartTime = DateTime.Now
            };
            PlayList playList = new PlayList{Party = party};
            Track track = new Track { Title = "Song1", Interpret = "Interpret", PlayLists = new List<PlayList>{playList}};
            votesController.AddVote(null, guest, playList, track);
        }

        [TestMethod]
        public void AddVote_NewCorrectVote_ShouldAddInDatabase()
        {
            IUnitOfWork unitOfWork = new MockUnitOfWork();
            VotesController votesController = new VotesController();
            Guest guest = new Guest { EmailAddress = "email@dummy.at", Id = 1};
            Party party = new Party
            {
                Guests = new List<Guest> { guest },
                EndTime = DateTime.Now,
                StartTime = DateTime.Now,
                Id = 1
            };
            PlayList playList = new PlayList { Party = party, Id = 1};
            Track track = new Track { Title = "Song1", Interpret = "Interpret", PlayLists = new List<PlayList> { playList } };
            unitOfWork.Parties.Insert(party);
            unitOfWork.Guests.Insert(guest);
            unitOfWork.PlayLists.Insert(playList);
            unitOfWork.Tracks.Insert(track);
            bool ok = votesController.AddVote(unitOfWork, guest, playList, track);
            Assert.IsTrue(ok);
            var votes = unitOfWork.Votes.Get();
            Assert.IsTrue(votes.Any(v => v.Track == track && v.PlayList == playList));
        }
    }
}
