using System;
using System.ComponentModel.DataAnnotations;

namespace Moviez.MoviezEngine.Models.RequestModels
{
    public class ProducerCreateRequest
    {
        [Required]
        public string? ProducerName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Company { get; set; }
        [Required]
        public string? GenderCode { get; set; }
        public string? Bio { get; set; }
    }
}

