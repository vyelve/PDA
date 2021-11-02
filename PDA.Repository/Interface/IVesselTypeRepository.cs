using PDA.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PDA.Repository
{
    public interface IVesselTypeRepository
    {
        Task<IEnumerable<VesselType>> GetVesselTypes();
        Task<VesselType> GetVesselTypeById(int Id);
        Task<VesselType> Add(VesselType model);
        Task<VesselType> Update(VesselType model);
        bool Delete(int Id);

    }
}
