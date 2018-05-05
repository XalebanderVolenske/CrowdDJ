using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrowdDj.BL.PoCos;

namespace CrowdDj.DAL
{
    public class MySeed
    {
        public static void Seed(CrowdDj.DAL.ApplicationDbContext context)
        {
            Administrator[] administrators = new Administrator[5];
            Party[] parties = new Party[5];
            Guest[] guests = new Guest[50];
            Track[] tracks = new Track[50];
            PlayList[] playLists = new PlayList[1];
            PartyTweet[] partyTweets = new PartyTweet[30];
            Vote[] votes = new Vote[5];

            //for (int i = 0; i < administrators.Length; i++)
            //{
            //    administrators[i] = new Administrator { UserName = $"admin{i+1}", Password = $"passme{(i+1) * 11}" };
            //    context.Administrators.Add(administrators[i]);
            //}

            //context.SaveChanges();

            for (int i = 0; i < guests.Length; i++)
            {
                string mail = $"user{i}@dummy.com";
                var guest = new Guest { EmailAddress = mail };
                context.Guests.Add(guest);
                guests[i] = guest;
            }

            var partyGuests = new List<PartyGuest>();
            int partyCode = 1;
            for (int i = 0; i < parties.Length; i++)
            {
                parties[i] = new Party { EndTime = DateTime.Now.AddDays(-1), Location = "Dahoam", Name = $"Party_{i}", StartTime = DateTime.Now.AddDays(-2) };
                var selectionDigit = i.ToString();
                var pGuests = guests.Where(g => g.EmailAddress.Contains(selectionDigit)).ToList();
                foreach (var guest in pGuests)
                {
                    var partyGuest = new PartyGuest{PartyCode = partyCode, Guest = guest, Party = parties[i]};
                    partyGuests.Add(partyGuest);
                    partyCode++;
                }
                context.Parties.Add(parties[i]);
            }

            context.PartyGuests.AddRange(partyGuests);

            for (int i = 0; i < partyTweets.Length; i++)
            {
                partyTweets[i] = new PartyTweet { Message = "Hallo! Die Party ist ein Hit!" }; ;
                partyTweets[i].PartyGuest = partyGuests.ElementAt(i);
                context.PartyTweets.Add(partyTweets[i]);
            }

            for (int i = 0; i < tracks.Length; i++)
            {
                tracks[i] = new Track { Interpret = $"Interpret{i + 10}", Title = $"Song{i}", Url = "www.mysongs.ate" };
            }

            playLists[0] = new PlayList
            {
                Tracks = tracks,
                Party = parties[0]
            };
            context.PlayLists.Add(playLists[0]);

            for (int i = 0; i < votes.Length; i++)
            {
                votes[i] = new Vote{ Track = tracks[i*2], Guest = guests[i], PlayList = playLists[0], TimeStamp = DateTime.Now.AddDays(-2).ToLocalTime()};
                context.Votes.Add(votes[i]);
            }
          
            int count = context.SaveChanges();
        }
    }
}
