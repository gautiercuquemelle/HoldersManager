using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace HoldersManager.Models
{
    public class Holder
    {        
        [Key]
        public int Id { get; set; }
        public int HolderTypeId { get; set; }
        public string HolderName { get; set; }
        public DateTime CreationDate { get; set; }
        public int NumberOfFrames { get; set; }
        public bool DiscardAfterDevelopment { get; set; }
        public string Comments { get; set; }

        // Navigation properties
        //public HolderType HolderType { get; set; }
    }
}
