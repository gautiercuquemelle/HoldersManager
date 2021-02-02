using System;
using System.Collections.Generic;
using System.Text;

namespace HoldersManager.Models
{
    public class FilmExposure
    {        
        public int Id { get; set; }
        public int FilmId { get; set; }
        public int CameraId { get; set; }
        public DateTime ExposureDateTime { get; set; }
        public decimal Aperture { get; set; }
        public decimal Exposure { get; set; }
        public int ExposureUnitId { get; set; }
        public string Location { get; set; }
        public string PreviewPath { get; set; }
        public string Comments { get; set; }
        public DateTime CreationDate { get; set; }

        // Navigation properties
        public Film Film { get; set; }
        public Camera Camera { get; set; }
        public ExposureUnit ExposureUnit { get; set; }
    }
}
