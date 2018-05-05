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
            Party[] parties = _unitOfWork.Parties.Get();
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
            var party = _unitOfWork.Parties.Get().Where(p => p.Id == id);
            IEnumerable<Guest> guests = _unitOfWork.Guests.Get(includeProperties: "Parties");
            if (!guests.Any())
            {
                return NotFound();
            }
            return Ok(guests);
        }

        [HttpGet("GetPlayListWithTracksOfParty/{id}")]
        public IActionResult GetPlayListWithTracksOfParty(int id)
        {
            var party = _unitOfWork.Parties.Get(includeProperties: "PlayList").SingleOrDefault(p => p.Id == id);
            var playList = _unitOfWork.PlayLists.Get(includeProperties: "Tracks").SingleOrDefault(t => party != null && t.Id == party.PlayList.Id);
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
            return Ok(party);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult UpdateParty(int id, [FromBody]Party party)
        {
            if (!ModelState.IsValid)
                return NotFound();

            var partyInDb = _unitOfWork.Parties.GetById(id);
            if (partyInDb == null)
                return NotFound();

            partyInDb.Name = party.Name;
            partyInDb.StartTime = party.StartTime;
            partyInDb.EndTime = party.EndTime;
            partyInDb.Location = party.Location;
            partyInDb.Guests = party.Guests;
            partyInDb.PlayList = party.PlayList;

            _unitOfWork.Parties.Update(partyInDb);
            _unitOfWork.SaveChanges();
            return Ok(party);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var partyInDb = _unitOfWork.Parties.GetById(id);

            if (partyInDb == null)
                 NotFound();
            _unitOfWork.Parties.Delete(partyInDb);
            _unitOfWork.SaveChanges();
            return Ok("Party deleted...");
        }
    }
}