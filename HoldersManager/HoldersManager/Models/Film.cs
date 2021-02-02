using System;
using System.Collections.Generic;
using System.Text;

namespace HoldersManager.Models
{
    public class Film
    {        
        public int Id { get; set; }
        public int FilmTypeId { get; set; }
        public string Comments { get; set; }

        // Navigation properties
        public FilmType FilmType { get; set; }
    }
}
