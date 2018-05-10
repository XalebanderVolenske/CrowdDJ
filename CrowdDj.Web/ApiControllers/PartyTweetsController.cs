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
    [Route("api/PartyTweets")]
    public class PartyTweetsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PartyTweetsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var partyTweets = _unitOfWork.PartyTweets.Get(includeProperties: "User");
            if (!partyTweets.Any())
            {
                return NotFound();
            }
            return Ok(partyTweets);
        }

        [HttpGet("GetPartyTweetsOfParty/{partyid}")]
        public IActionResult GetPartyTweetsOfParty(int partyid)
        {
            var partyTweets = _unitOfWork.PartyTweets.Get(includeProperties: "Party").Where(p => p.PartyGuest.PartyId == partyid);
            if (!partyTweets.Any())
            {
                return NotFound();
            }
            return Ok(partyTweets);
        }

        [HttpPost]
        public IActionResult Post([FromBody] PartyTweet partyTweet)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            _unitOfWork.PartyTweets.Insert(partyTweet);
            _unitOfWork.SaveChanges();
            return Ok(partyTweet);
        }
    }
}