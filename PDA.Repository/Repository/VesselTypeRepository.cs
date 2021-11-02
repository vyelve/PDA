using Microsoft.EntityFrameworkCore;
using PDA.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDA.Repository
{
    public class VesselTypeRepository : IVesselTypeRepository
    {
        private readonly PDADbContext _appDBContext;

        public VesselTypeRepository(PDADbContext context)
        {
            _appDBContext = context ?? throw new ArgumentNullException(nameof(context));

        }
        public async Task<IEnumerable<VesselType>> GetVesselTypes()
        {
            return await _appDBContext.VesselTypes.ToListAsync();
        }
        public async Task<VesselType> GetVesselTypeById(int Id)
        {
            return await _appDBContext.VesselTypes.FindAsync(Id);
        }

        public async Task<VesselType> Add(VesselType model)
        {
            _appDBContext.VesselTypes.Add(model);
            await _appDBContext.SaveChangesAsync();
            return model;
        }
        public async Task<VesselType> Update(VesselType model)
        {
            _appDBContext.Entry(model).State = EntityState.Modified;
            await _appDBContext.SaveChangesAsync();
            return model;
        }

        public bool Delete(int Id)
        {
            bool result = false;
            var _vesselType = _appDBContext.VesselTypes.Find(Id);
            if (_vesselType != null)
            {
                _appDBContext.Entry(_vesselType).State = EntityState.Deleted;
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
