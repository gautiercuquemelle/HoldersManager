using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace HoldersManager.Models
{
    public class HolderFilm
    {        
        [Key]
        public int Id { get; set; }
        public int FilmId { get; set; }
        public int HolderId { get; set; }
        public DateTime CreationDate { get; set; }

        // Naviation properties
        public Film Film { get; set; }
        public Holder Holder { get; set; }
    }
}
