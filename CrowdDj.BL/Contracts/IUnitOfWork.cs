using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrowdDj.BL.PoCos;
using CrowdDj.BL.Contracts;

namespace CrowdDj.BL.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Administrator> Administrators { get; }
        IGenericRepository<Guest> Guests { get; }
        IGenericRepository<Party> Parties { get; }
        IGenericRepository<PartyTweet> PartyTweets { get; }
        IGenericRepository<PlayList> PlayLists { get; }
        IGenericRepository<Track> Tracks { get; }
        IGenericRepository<Vote> Votes { get; }
        IGenericRepository<PartyGuest> PartyGuests { get; }

        void SaveChanges();

        void DeleteDatabase();
    }
}
