using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CrowdDj.BL.Contracts;
using CrowdDj.BL.PoCos;
using CrowdDj.DAL;
using CrowdDj.WPF.WindowControllers;

namespace CrowdDj.WPF.ViewModels
{
    class GuestViewModel : BaseViewModel
    {
        private Track _selectedTrack;
        private ListTrack _selectedVoteTrack;
        private Track _selectedRecommendedTrack;
        private string _textPartyTweet;

        public GuestViewModel() : base(null)
        {
        }

        public GuestViewModel(IWindowController windowController, PartyGuest partyGuest) : base(windowController)
        {
            PartyPlayList = new PlayList();
            SelectedVoteTrack = new ListTrack();
            TracksOfPartyPlayList = new List<ListTrack>();
            PartyTweets = new List<PartyTweet>();
            _selectedRecommendedTrack = new Track();

            TextPartyTweet = "";

            LoadCommands();
            LoadData(partyGuest);
        }

        public struct ListTrack
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Interpret { get; set; }
            public int Votes { get; set; }
            public PartyGuest RecommendedBy { get; set; }
        }

        public string NewTextPartyTweet { get; set; }
        
        public PartyGuest LoggedPartyGuest { get; set; }

        public PlayList PartyPlayList { get; set; }

        public List<ListTrack> TracksOfPartyPlayList { get; set; }

        public ObservableCollection<Track> TracksNotInPlayList { get; set; }

        public List<PartyTweet> PartyTweets { get; set; }

        public string[] TextPartyTweets { get; set; }

        public string TextPartyTweet
        {
            get { return _textPartyTweet; }
            set
            {
                _textPartyTweet = value;
                OnPropertyChanged();
            }
        }

        public Track SelectedTrack
        {
            get { return _selectedTrack; }
            set
            {
                _selectedTrack = value;
                OnPropertyChanged();
            }
        }

        public ListTrack SelectedVoteTrack
        {
            get { return _selectedVoteTrack; }
            set
            {
                _selectedVoteTrack = value;
                OnPropertyChanged();
            }
        }

        public Track SelectedRecommendedTrack
        {
            get { return _selectedRecommendedTrack; }
            set
            {
                _selectedRecommendedTrack = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand CommandVoteForTrack { get; set; }
        public RelayCommand CommandRecommendedTrack { get; set; }
        public RelayCommand CommandSendPartyTweet { get; set; }

        private void LoadCommands()
        {
            CommandVoteForTrack = new RelayCommand(_ => VoteForTrack(), _ => true);
            CommandRecommendedTrack = new RelayCommand(_ => RecommendedTrack(), _ => true);
            CommandSendPartyTweet = new RelayCommand(SendPartyTweet, c => true);
        }

        private void LoadData(PartyGuest partyGuest)
        {
            using (IUnitOfWork unitOfWork = new UnitOfWork())
            {
                LoggedPartyGuest = unitOfWork.PartyGuests.Get(includeProperties:"Guest,PartyTweets")
                                                         .SingleOrDefault(pg => pg.Id == partyGuest.Id);
            }
            LoadPlayListWithTracks();
            LoadPartyTweets();
            LoadAllTracks();
        }

        private void LoadPlayListWithTracks()
        {
            var tracksOfPartyPlayList = new List<ListTrack>();
            using (IUnitOfWork unitOfWork = new UnitOfWork())
            {
                PartyPlayList = unitOfWork.PlayLists.Get(includeProperties: "Party,Tracks")
                    .SingleOrDefault(p => p.Party.Id == LoggedPartyGuest.PartyId);
                if (PartyPlayList != null)
                {
                    foreach (Track t in PartyPlayList.Tracks)
                    {
                        var recommendedByGuest = unitOfWork.Tracks
                            .Get(includeProperties: "Votes,RecommendedByGuest").SingleOrDefault(rt => rt.Id == t.Id)
                            ?.RecommendedByGuest;
                        if (recommendedByGuest != null)
                        {
                            tracksOfPartyPlayList.Add(item: new ListTrack
                            {
                                Id = t.Id,
                                Interpret = t.Interpret,
                                Title = t.Title,
                                Votes = t.Votes.Count,
                                RecommendedBy = recommendedByGuest
                            });
                        }
                        else
                        {
                            tracksOfPartyPlayList.Add(item: new ListTrack
                            {
                                Id = t.Id,
                                Interpret = t.Interpret,
                                Title = t.Title,
                                Votes = t.Votes.Count
                            });
                        }
                    }
                    tracksOfPartyPlayList.Sort((t1, t2) => t2.Votes.CompareTo(t1.Votes));
                    TracksOfPartyPlayList = tracksOfPartyPlayList;
                }
            }
        }

        private void LoadPartyTweets()
        {
            using (IUnitOfWork unitOfWork = new UnitOfWork())
            {

                PartyTweets = unitOfWork.PartyTweets.Get(includeProperties:"PartyGuest")
                                                    .Where(pt => pt.PartyGuest.PartyId == LoggedPartyGuest.PartyId).ToList();
                if (PartyTweets != null)
                {
                    foreach (PartyTweet p in PartyTweets)
                    {
                        TextPartyTweet += unitOfWork.Guests.GetById(p.PartyGuest.GuestId).EmailAddress
                                          + " hat geschrieben: \n"
                                          + p.Message + " \n\n";
                    }
                }
            }
        }

        private void LoadAllTracks()
        {
            using (IUnitOfWork unitOfWork = new UnitOfWork())
            {
                var allSongs = unitOfWork.Tracks.Get(includeProperties: "PlayLists")
                    .Where(t => !t.PlayLists.Contains(PartyPlayList)).ToList();

                if (PartyPlayList != null)
                    foreach (Track t in allSongs)
                    {
                        TracksOfPartyPlayList.Add(new ListTrack
                        {
                            Interpret = t.Interpret,
                            Title = t.Title
                        });
                    }
            }
        }

        private void VoteForTrack()
        {
            using (IUnitOfWork unitOfWork = new UnitOfWork())
            {
                Vote vote = new Vote
                {
                    GuestId = LoggedPartyGuest.Id,
                    PlayListId = PartyPlayList.Id,
                    TrackId = SelectedVoteTrack.Id,
                    TimeStamp = DateTime.Now.ToLocalTime(),
                };
                var checkVote = unitOfWork.Votes.Get(includeProperties: "Guest,PlayList,Track")
                    .SingleOrDefault(cv => cv.PlayList.Id == PartyPlayList.Id &&
                                           cv.Track.Id == SelectedVoteTrack.Id &&
                                           cv.GuestId == LoggedPartyGuest.GuestId);
                if (checkVote == null)
                {                      
                    unitOfWork.Votes.Insert(vote);
                    unitOfWork.SaveChanges();
                    MessageBox.Show("Successfully voted!");
                    LoadPlayListWithTracks();
                }
                else
                {
                    MessageBox.Show("Already voted!");
                }
            }
        }

        private void RecommendedTrack()
        {
            if (SelectedRecommendedTrack != null)
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    var checkRecommandation = unitOfWork.Tracks.Get(includeProperties: "Guest,PlayList,Track")
                        .SingleOrDefault(t => t.RecommendedByGuest.Id == LoggedPartyGuest.Id);
                    if (checkRecommandation == null)
                    {
                        SelectedRecommendedTrack.RecommendedByGuest = LoggedPartyGuest;
                        unitOfWork.Tracks.Update(SelectedRecommendedTrack);
                        unitOfWork.SaveChanges();
                        MessageBox.Show("Song successfully recommended!");
                    }
                    else
                    {
                        MessageBox.Show("You already recommended this Song!");
                    }
                }
            }
        }

        private void SendPartyTweet(object obj)
        {
            using (IUnitOfWork unitOfWork = new UnitOfWork())
            {
                var partyTweet = new PartyTweet{Message = NewTextPartyTweet, PartyGuest = LoggedPartyGuest};
                LoggedPartyGuest.PartyTweets.Add(partyTweet);
                unitOfWork.PartyGuests.Update(LoggedPartyGuest);
                unitOfWork.PartyTweets.Insert(partyTweet);
                try
                {
                    unitOfWork.SaveChanges();
                    TextPartyTweet = ""; OnPropertyChanged();
                    LoadPartyTweets();
                }
                catch (ValidationException ex)
                {
                    return;
                }
            }
        }
    }
}
