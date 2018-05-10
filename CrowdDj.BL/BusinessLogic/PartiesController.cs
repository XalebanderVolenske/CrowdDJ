using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrowdDj.BL.Contracts;
using CrowdDj.BL.PoCos;

namespace CrowdDj.BL.BusinessLogic
{
    public class PartiesController
    {
        public bool AddParty(IUnitOfWork unitOfWork, Party party)
        {
            if (party == null || unitOfWork == null)
            {
                throw new ArgumentNullException();
            }

            if (unitOfWork.Parties.Get(p => p.Id == party.Id).Length != 0)
            {
                return false;
            }

            Party newParty = new Party
            {
                Name = party.Name,
                Location = party.Location,
                StartTime = party.StartTime,
                EndTime = party.EndTime,
                PlayList = party.PlayList,
                Guests = party.Guests
            };
            unitOfWork.Parties.Insert(newParty);
            unitOfWork.SaveChanges();
            return true;
        }
    }
}
