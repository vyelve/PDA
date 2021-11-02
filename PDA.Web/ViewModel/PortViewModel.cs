using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PDA.Web.ViewModel
{
    public class PortViewModel
    {
        public int PortID { get; set; }

        [Required(ErrorMessage = "Port Name Required")]
        public string PortName { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public IEnumerable<PortViewModel> TblPort { get; set; }
        public string Message { get; set; }

    }
}
