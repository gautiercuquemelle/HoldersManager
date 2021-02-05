using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace HoldersManager.Models
{
    public class FilmDevelopment
    {        
        [Key]
        public int Id { get; set; }
        public int FilmId { get; set; }
        public int DeveloperId { get; set; }
        public int DevelopmentDuration { get; set; }
        public DateTime DevelopmentDateTime { get; set; }
        public string PreviewPath { get; set; }
        public string Comments { get; set; }
        public DateTime CreationDate { get; set; }

        // Navigation properties
        public Film Film { get; set; }
        public Developer Developer { get; set; }
    }
}
