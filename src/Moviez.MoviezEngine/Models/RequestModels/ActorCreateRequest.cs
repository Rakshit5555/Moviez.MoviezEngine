using System;
using System.ComponentModel.DataAnnotations;

namespace Moviez.MoviezEngine.Models.RequestModels
{
    public class ActorCreateRequest
    {
        
        [Required]
        public string? ActorName { get; set; }
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string? GenderCode { get; set; }
        public string? Bio { get; set; }
    }
}

