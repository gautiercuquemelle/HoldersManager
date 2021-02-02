using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace HoldersManager.Models
{
    public class Film
    {        
        [Key]
        public int Id { get; set; }
        public int FilmTypeId { get; set; }
        public string Comments { get; set; }

        // Navigation properties
        public FilmType FilmType { get; set; }
    }
}
