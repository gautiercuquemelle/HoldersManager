using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HoldersManager.Models
{
    public class BaseConfigModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public int? Order { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
