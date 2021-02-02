using System;
using System.Collections.Generic;
using System.Text;

namespace HoldersManager.Models
{
    public class Holder
    {        
        public int Id { get; set; }
        public int HolderTypeId { get; set; }
        public string HolderName { get; set; }
        public DateTime CreationDate { get; set; }
        public int FrameNumber { get; set; }
        public bool DiscardAfterDevelopment { get; set; }
        public string Comments { get; set; }

        // Navigation properties
        public HolderType HolderType { get; set; }
    }
}
