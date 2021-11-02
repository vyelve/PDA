using PDA.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PDA.Repository
{
    public interface IPortRepository
    {
        Task<IEnumerable<Port>> GetPorts();
        Task<Port> GetPortById(int Id);
        Task<Port> Add(Port model);
        Task<Port> Update(Port model);
        bool Delete(int Id);
    }
}
