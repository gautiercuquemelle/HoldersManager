using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace HoldersManager.Models
{
    public class FilmType
    {        
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int ISO { get; set; }
        public string Comments { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
