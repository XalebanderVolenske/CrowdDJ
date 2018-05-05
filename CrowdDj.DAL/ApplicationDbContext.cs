using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Text;
using CrowdDj.BL.PoCos;

namespace CrowdDj.DAL
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<Guest> Guests { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Party> Parties { get; set; }
        public DbSet<PlayList> PlayLists { get; set; }
        public DbSet<PartyTweet> PartyTweets { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<PartyGuest> PartyGuests { get; set; }

        /// <summary>
        /// Connectionstring kommt entweder über den Namen (Verweis auf 
        /// XML-Configdatei) oder direkt als voller ConnectionString
        /// </summary>
        /// <param name="nameOrConnectionString"></param>
        public ApplicationDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        public ApplicationDbContext() : base("name=DefaultConnection")
        {

        }


    }
}
