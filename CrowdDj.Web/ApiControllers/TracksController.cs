using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrowdDj.BL.PoCos;
using CrowdDj.BL.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CrowdDj.Web.ApiControllers
{
    [Produces("application/json")]
    [Route("api/Tracks")]
    public class TracksController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TracksController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<Track> tracks = _unitOfWork.Tracks.Get();
            if (!tracks.Any())
            {
                return NotFound();
            }
            return Ok(tracks);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTrack(int id, [FromBody]Track track)
        {
            if (!ModelState.IsValid)
                return NotFound();

            var trackInDb = _unitOfWork.Tracks.GetById(id);
            if (trackInDb == null)
                return NotFound();

            trackInDb.Interpret = track.Interpret;
            trackInDb.PlayLists = track.PlayLists;
            trackInDb.RecommendedByGuest = track.RecommendedByGuest;
            trackInDb.Title = track.Title;
            trackInDb.Url = track.Url;
            trackInDb.Votes = track.Votes;

            _unitOfWork.Tracks.Update(trackInDb);
            _unitOfWork.SaveChanges();
            return Ok(trackInDb);
        }
    }
}