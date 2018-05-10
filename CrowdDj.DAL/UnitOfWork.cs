using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using CrowdDj.BL.PoCos;
using CrowdDj.BL.Contracts;

namespace CrowdDj.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// ConnectionString kommt aus den appsettings.json
        /// </summary>
        public UnitOfWork(string connectionString)
        {
            _dbContext = new ApplicationDbContext();
            Guests = new GenericRepository<Guest>(_dbContext);
            Parties = new GenericRepository<Party>(_dbContext);
            Administrators = new GenericRepository<Administrator>(_dbContext);
            PlayLists = new GenericRepository<PlayList>(_dbContext);
            Tracks = new GenericRepository<Track>(_dbContext);
            Votes = new GenericRepository<Vote>(_dbContext);
            PartyTweets = new GenericRepository<PartyTweet>(_dbContext);
            PartyGuests = new GenericRepository<PartyGuest>(_dbContext);
        }

        /// <summary>
        /// ConnectionString kommt aus den XML-config-Dateien
        /// </summary>
        public UnitOfWork() : this("name=DefaultConnection")
        {
        }



        public void DeleteDatabase()
        {
            _dbContext.Database.Delete();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public IGenericRepository<Guest> Guests { get; }
        public IGenericRepository<Party> Parties { get; }
        public IGenericRepository<Administrator> Administrators { get; }
        public IGenericRepository<PlayList> PlayLists { get; set; }
        public IGenericRepository<Track> Tracks { get; set; }
        public IGenericRepository<Vote> Votes { get; set; }
        public IGenericRepository<PartyTweet> PartyTweets { get; set; }
        public IGenericRepository<PartyGuest> PartyGuests { get; set; }

        public void SyncChangeTrackerCache()
        {
            var entries = _dbContext.ChangeTracker.Entries().ToList();
            entries.ForEach(entry => entry.Reload());
        }

        public void SaveChanges()
        {
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

    }

}
