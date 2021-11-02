using Microsoft.EntityFrameworkCore;
using PDA.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PDA.Repository
{
    public class TerminalRepository : ITerminalRepository
    {
        private readonly PDADbContext _appDBContext;

        public TerminalRepository(PDADbContext context)
        {
            _appDBContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Terminal>> GetTerminals()
        {
            return await _appDBContext.Terminals.ToListAsync();
        }

        public async Task<Terminal> GetTerminalById(int Id)
        {
            return await _appDBContext.Terminals.FindAsync(Id);
        }

        public async Task<Terminal> Add(Terminal model)
        {
            _appDBContext.Terminals.Add(model);
            await _appDBContext.SaveChangesAsync();
            return model;
        }
        public async Task<Terminal> Update(Terminal model)
        {
            _appDBContext.Entry(model).State = EntityState.Modified;
            await _appDBContext.SaveChangesAsync();
            return model;
        }

        public bool Delete(int Id)
        {
            bool result = false;
            var _terminal = _appDBContext.Terminals.Find(Id);
            if (_terminal != null)
            {
                _appDBContext.Entry(_terminal).State = EntityState.Deleted;
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
