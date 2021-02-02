using System;
using System.Collections.Generic;
using System.Text;

namespace HoldersManager.Models
{
    public class FilmType
    {        
        public int Id { get; set; }
        public string Name { get; set; }
        public int ISO { get; set; }
        public string Comments { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
