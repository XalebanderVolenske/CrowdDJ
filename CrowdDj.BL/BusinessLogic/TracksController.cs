using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrowdDj.BL.Contracts;
using CrowdDj.BL.PoCos;

namespace CrowdDj.BL.BusinessLogic
{
    public class TracksController
    {
        public bool AddTrack(IUnitOfWork unitOfWork, Track track)
        {
            if (track == null || unitOfWork == null)
            {
                throw new ArgumentNullException();
            }
            if (unitOfWork.Tracks.Get(t => t.Id == track.Id).Length != 0)
            {
                return false;
            }

            Track newTrack = new Track
            {
                Interpret = track.Interpret,
                Title =  track.Title,
                Url = track.Url,
                RecommendedByGuest = track.RecommendedByGuest
            };
            unitOfWork.Tracks.Insert(track);
            unitOfWork.SaveChanges();
            return true;
        }

        public bool DeleteTrack(IUnitOfWork unitOfWork, Track track)
        {
            if (track == null || unitOfWork == null)
            {
                throw new ArgumentNullException();
            }
            if (unitOfWork.Tracks.Get(t => t.Id == track.Id).Length == 0)
            {
                return false;
            }
            unitOfWork.Tracks.Delete(track);
            unitOfWork.SaveChanges();
            return true;
        }
    }
}
