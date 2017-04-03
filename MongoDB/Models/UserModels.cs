using System;
using System.ComponentModel.DataAnnotations;

namespace MongoDB.Models
{
    public class UserModels
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime Created { get; set; }
    }
}