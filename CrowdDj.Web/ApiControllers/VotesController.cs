using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrowdDj.BL.PoCos;
using CrowdDj.BL.Contracts;
using CrowdDj.Web.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CrowdDj.Web.ApiControllers
{
    [Produces("application/json")]
    [Route("api/Votes")]
    public class VotesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VotesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<Vote> votes = _unitOfWork.Votes.Get(includeProperties: "Guest,Track");
            if (!votes.Any())
            {
                return NotFound();
            }
            return Ok(votes);
        }

        // GET api/values/5
        [HttpGet("GetVotesOfTrack/{id}")]
        public IActionResult GetVotesOfTrack(int trackId)
        {
            var votes = _unitOfWork.Votes.Get(includeProperties: "Track,Guest").Where(v => v.Track.Id == trackId);
            if (!votes.Any())
            {
                return NotFound();
            }
            return Ok(votes);
        }

        [HttpGet("GetVotesOfTrackInPlayList/{playListId}")]
        public IActionResult GetVotesOfTrackInPlayList(int playListId)
        {
            var votes = _unitOfWork.Votes.Get(includeProperties: "Track,Guest,PlayList").Where(v => v.PlayList.Id == playListId);
            if (!votes.Any())
            {
                return NotFound();
            }
            return Ok(votes);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]VoteDto vote)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_unitOfWork.Votes.Get(v =>
                    v.GuestId == vote.GuestId && v.PlayListId == vote.PlayListId && v.TrackId == vote.TrackId).Length ==
                0)
            {

                Vote newVote = new Vote{Guest = _unitOfWork.Guests.GetById(vote.GuestId), PlayList = _unitOfWork.PlayLists.GetById(vote.PlayListId),
                    Track = _unitOfWork.Tracks.GetById(vote.TrackId), TimeStamp = DateTime.Now};
                _unitOfWork.Votes.Insert(newVote);
                _unitOfWork.SaveChanges();
                return Created($"api/Votes/{newVote.Id}", newVote);
            }

            return BadRequest();
        }

    }
}