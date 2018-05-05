using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrowdDj.BL.Contracts;
using CrowdDj.BL.PoCos;
using CrowdDj.BLTests;

namespace CrowdDj.BLTests
{
    public class MockUnitOfWork : IUnitOfWork
    {
        public MockUnitOfWork()
        {
            Guests = new MockGenerciRepository<Guest>();
            Parties = new MockGenerciRepository<Party>();
            Administrators = new MockGenerciRepository<Administrator>();
            PlayLists = new MockGenerciRepository<PlayList>();
            Tracks = new MockGenerciRepository<Track>();
            Votes = new MockGenerciRepository<Vote>();
            PartyTweets = new MockGenerciRepository<PartyTweet>();
            PartyGuests = new MockGenerciRepository<PartyGuest>();

        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IGenericRepository<Administrator> Administrators { get; }
        public IGenericRepository<Guest> Guests { get; }
        public IGenericRepository<Party> Parties { get; }
        public IGenericRepository<PartyTweet> PartyTweets { get; }
        public IGenericRepository<PlayList> PlayLists { get; }
        public IGenericRepository<Track> Tracks { get; }
        public IGenericRepository<Vote> Votes { get; }
        public IGenericRepository<PartyGuest> PartyGuests { get; }
        public void SaveChanges()
        {
        }

        public void DeleteDatabase()
        {
            throw new NotImplementedException();
        }
    }
}
