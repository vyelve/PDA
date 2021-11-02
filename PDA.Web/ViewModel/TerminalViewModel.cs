using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PDA.Web.ViewModel
{
    public class TerminalViewModel
    {
        public int TerminalID { get; set; }
        [Required(ErrorMessage = "Terminal Name Required")]
        public string TerminalName { get; set; }

        [Required(ErrorMessage = "Port Required")]
        public int PortID { get; set; }
        public string PortName { get; set; }

        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public IEnumerable<TerminalViewModel> TblTerminal { get; set; }
        public string Message { get; set; }
    }
}
