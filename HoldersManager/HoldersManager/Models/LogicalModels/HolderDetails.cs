﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HoldersManager.Models
{
    public class HolderDetails
    {
        public int Id { get; set; }
        //public int HolderTypeId { get; set; }
        public string HolderTypeName { get; set; }
        public string HolderName { get; set; }
        public DateTime CreationDate { get; set; }
        public int NumberOfFrames { get; set; }
        public int NumberOfLoadedFrames { get; set; }
        public int NumberOfExposedFrames { get; set; }
        public decimal LoadingRatio { get => NumberOfFrames != 0 ? (decimal)NumberOfLoadedFrames / (decimal)NumberOfFrames : 0; }
        public decimal ExposureRatio { get => NumberOfFrames != 0 ? (decimal)NumberOfExposedFrames / (decimal)NumberOfFrames : 0; }
        public bool DiscardAfterDevelopment { get; set; }
        public string Comments { get; set; }
        public bool IsLoaded { get => NumberOfLoadedFrames > 0; }
        public bool IsExposed { get => NumberOfExposedFrames > 0; }


    }
}
