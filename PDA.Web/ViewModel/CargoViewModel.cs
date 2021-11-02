using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PDA.Web.ViewModel
{
    public class CargoViewModel
    {
        public int CargoID { get; set; }
        [Required(ErrorMessage = "Cargo Name Required")]
        public string CargoName { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public IEnumerable<CargoViewModel> TblCargo { get; set; }
        public string Message { get; set; }
    }
}
