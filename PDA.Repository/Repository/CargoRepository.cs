using Microsoft.EntityFrameworkCore;
using PDA.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDA.Repository
{
    public class CargoRepository : ICargoRepository
    {
        private readonly PDADbContext _appDBContext;

        public CargoRepository(PDADbContext context)
        {
            _appDBContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Cargo>> GetCargos()
        {
            return await _appDBContext.Cargos.ToListAsync();
        }

        public async Task<Cargo> GetCargoById(int Id)
        {
            return await _appDBContext.Cargos.FindAsync(Id);
        }

        public async Task<Cargo> Add(Cargo model)
        {
            _appDBContext.Cargos.Add(model);
            await _appDBContext.SaveChangesAsync();
            return model;
        }
        public async Task<Cargo> Update(Cargo model)
        {
            _appDBContext.Entry(model).State = EntityState.Modified;
            await _appDBContext.SaveChangesAsync();
            return model;
        }

        public bool Delete(int Id)
        {
            bool result = false;
            var _cargo = _appDBContext.Cargos.Find(Id);
            if (_cargo != null)
            {
                _appDBContext.Entry(_cargo).State = EntityState.Deleted;
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
