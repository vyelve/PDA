using PDA.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PDA.Repository
{
    public interface ITerminalRepository
    {
        Task<IEnumerable<Terminal>> GetTerminals();
        Task<Terminal> GetTerminalById(int Id);
        Task<Terminal> Add(Terminal model);
        Task<Terminal> Update(Terminal model);
        bool Delete(int Id);
    }
}
