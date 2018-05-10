using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrowdDj.BL.PoCos;
using CrowdDj.BL.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Isam.Esent.Interop;

namespace CrowdDj.Web.ApiControllers
{
    [Produces("application/json")]
    [Route("api/Parties")]
    public class PartiesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PartiesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var parties = _unitOfWork.Parties.Get();
            if (!parties.Any())
            {
                return NotFound();
            }
            return Ok(parties);
        }

        // GET User by partycode
        // GET api/values/5
        [HttpGet("{partycode}")]
        public IActionResult Get(int partycode)
        {
            var partyGuest = _unitOfWork.PartyGuests.Get(includeProperties: "Guest").SingleOrDefault(pg => pg.PartyCode == partycode);
            if (partyGuest == null)
            {
                return NotFound();
            }
            return Ok(partyGuest.Guest);
        }

        [HttpGet("AllGuestsOfParty/{id}")]
        public IActionResult GetAllGuestsOfParty(int id)
        {
            var guests = _unitOfWork.PartyGuests.Get(includeProperties: "Party").Where(pg => pg.PartyId == id);
            if (!guests.Any())
            {
                return NotFound();
            }
            return Ok(guests);
        }

        [HttpGet("GetPlayListWithTracksOfParty/{id}")]
        public IActionResult GetPlayListWithTracksOfParty(int id)
        {
            var playList = _unitOfWork.PlayLists.Get(includeProperties: "Party,Tracks,Votes").SingleOrDefault(pl => pl.Party.Id == id);
            if (playList == null)
            {
                return NotFound();
            }

            return Ok(playList);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]Party party)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            _unitOfWork.Parties.Insert(party);
            _unitOfWork.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult UpdateParty(int id, [FromBody]Party party)
        {
            if (!ModelState.IsValid)
                return NotFound();

            if (id != party.Id)
            {
                return BadRequest();
            }

            try
            {
                var partyInDb = _unitOfWork.Parties.GetById(party.Id);
                if (partyInDb != null)
                {
                    partyInDb.Name = party.Name;
                    partyInDb.StartTime = party.StartTime;
                    partyInDb.EndTime = party.EndTime;
                    partyInDb.Location = party.Location;
                    partyInDb.Guests = party.Guests;
                    partyInDb.PlayList = party.PlayList;

                    _unitOfWork.Parties.Update(partyInDb);
                    _unitOfWork.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return NotFound("No Record Found against this Id...");
            }
            return Ok(party);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var partyInDb = _unitOfWork.Parties.GetById(id);
            _unitOfWork.Parties.Delete(partyInDb);
            _unitOfWork.SaveChanges();
            return Ok("Party deleted...");
        }
    }
}