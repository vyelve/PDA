using Microsoft.EntityFrameworkCore;
using PDA.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDA.Repository
{
    public class PilotageRepository : IPilotageRepository
    {
        private readonly PDADbContext _appDBContext;

        public PilotageRepository(PDADbContext context)
        {
            _appDBContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Pilotage>> GetPilotages()
        {
            return await _appDBContext.Pilotages.ToListAsync();
        }
        public async Task<Pilotage> GetPilotageById(int Id)
        {
            return await _appDBContext.Pilotages.FindAsync(Id);
        }

        public async Task<Pilotage> Add(Pilotage model)
        {
            _appDBContext.Pilotages.Add(model);
            await _appDBContext.SaveChangesAsync();
            return model;
        }
        public async Task<Pilotage> Update(Pilotage model)
        {
            _appDBContext.Entry(model).State = EntityState.Modified;
            await _appDBContext.SaveChangesAsync();
            return model;
        }
        public bool Delete(int Id)
        {
            bool result = false;
            var _pilotage = _appDBContext.Pilotages.Find(Id);
            if (_pilotage != null)
            {
                _appDBContext.Entry(_pilotage).State = EntityState.Deleted;
                _appDBContext.SaveChanges();
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
       
    }
}
