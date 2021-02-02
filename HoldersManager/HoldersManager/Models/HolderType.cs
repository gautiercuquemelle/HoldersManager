using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HoldersManager.Models
{
    public class HolderType
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }

        //public IEnumerable<Holder> Holders { get; set; }
    }
}
