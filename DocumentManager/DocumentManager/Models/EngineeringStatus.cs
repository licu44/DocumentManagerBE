﻿using System.ComponentModel.DataAnnotations;

namespace DocumentManager.Models
{
    public class EngineeringStatus
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Status { get; set; }
    }
}
