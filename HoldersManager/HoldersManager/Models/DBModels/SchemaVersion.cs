﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HoldersManager.Models
{
    public class SchemaVersion
    {
        [Key]
        public int VersionNumber { get; set; }
    }
}
