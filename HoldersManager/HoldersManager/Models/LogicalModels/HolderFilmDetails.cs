using System;
using System.Collections.Generic;
using System.Text;

namespace HoldersManager.Models
{
    public class HolderFilmDetails
    {
        public int Id { get; set; }

        // holder infos
        public int HolderId { get; set; }
        public string HolderName { get; set; }

        // Film Infos
        public int FilmId { get; set; }
        public string FilmName { get; set; }
        public int ISO { get; set; }
        public DateTime? ExposureDateTime { get; set; }
        public string Comments { get; set; }
    }
}
