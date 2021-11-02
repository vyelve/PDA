using PDA.Web.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PDA.Web.ViewModel
{
    public class PilotageViewModel
    {
        public int PilotageID { get; set; }

        [Required(ErrorMessage = "Port Required")]
        public int PortId { get; set; }
        public string PortName { get; set; }

        public string Stage { get; set; }

        public int PilotType { get; set; }

        [Required(ErrorMessage = "Pilot Type Required")]
        public PilotTypeEnum PilotTypeEnum { get; set; }

        [Required(ErrorMessage = "Fixed Tariff Required")]
        public long Fixed_Tariff { get; set; }

        [Required(ErrorMessage = "Tariff Required")]
        public decimal Tariff { get; set; }

        [Required(ErrorMessage = "Range Trom Required")]
        public long Range1 { get; set; }

        [Required(ErrorMessage = "Range To Required")]
        public long Range2 { get; set; }

        //public bool IsActive { get; set; }
        public string CreatedBy { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
        public IEnumerable<PilotageViewModel> TblPilotage { get; set; }
        public string Message { get; set; }
    }
}
