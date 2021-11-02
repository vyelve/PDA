using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PDA.Entities
{
    [Table("Terminal")]
    public class Terminal
    {
        [Key]
        public int TerminalID { get; set; }
        public string TerminalName { get; set; }
        public int PortID { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }

    }
}
