using PDA.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PDA.Repository
{
    public interface ICargoRepository
    {
        Task<IEnumerable<Cargo>> GetCargos();
        Task<Cargo> GetCargoById(int Id);
        Task<Cargo> Add(Cargo model);
        Task<Cargo> Update(Cargo model);
        bool Delete(int Id);
    }
}
