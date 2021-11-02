using Microsoft.EntityFrameworkCore;
using PDA.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDA.Repository
{
    public class PortRepository : IPortRepository
    {

        private readonly PDADbContext _appDBContext;

        public PortRepository(PDADbContext context)
        {
            _appDBContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Port>> GetPorts()
        {
            return await _appDBContext.Ports.ToListAsync();
        }
        public async Task<Port> GetPortById(int Id)
        {
            return await _appDBContext.Ports.FindAsync(Id);
        }

        public async Task<Port> Add(Port model)
        {
            _appDBContext.Ports.Add(model);
            await _appDBContext.SaveChangesAsync();
            return model;
        }

        public async Task<Port> Update(Port model)
        {
            _appDBContext.Entry(model).State = EntityState.Modified;
            await _appDBContext.SaveChangesAsync();
            return model;
        }

        public bool Delete(int Id)
        {
            bool result = false;
            var _port = _appDBContext.Ports.Find(Id);
            if (_port != null)
            {
                _appDBContext.Entry(_port).State = EntityState.Deleted;
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
