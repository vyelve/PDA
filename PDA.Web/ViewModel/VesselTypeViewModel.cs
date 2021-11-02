using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PDA.Web.ViewModel
{
    public class VesselTypeViewModel
    {
        public int VesselTypeID { get; set; }
        [Required(ErrorMessage = "Vessel Type Name Required")]
        public string VesselTypeName { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public IEnumerable<VesselTypeViewModel> TblVesselType { get; set; }
        public string Message { get; set; }
    }
}
