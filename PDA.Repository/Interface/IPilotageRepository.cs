using PDA.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PDA.Repository
{
    public interface IPilotageRepository
    {
        Task<IEnumerable<Pilotage>> GetPilotages();
        Task<Pilotage> GetPilotageById(int Id);
        Task<Pilotage> Add(Pilotage model);
        Task<Pilotage> Update(Pilotage model);
        bool Delete(int Id);
    }
}
