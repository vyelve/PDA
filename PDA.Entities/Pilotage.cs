using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PDA.Entities
{
    [Table("Pilotage")]
    public class Pilotage
    {
        [Key]
        public int PilotageID { get; set; }
        public int PortId { get; set; }
        public string Stage { get; set; }
        public int PilotType { get; set; }
        public long Fixed_Tariff { get; set; }
        public decimal Tariff { get; set; }
        public long Range1 { get; set; }
        public long Range2 { get; set; }
        //public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }

    }
}
