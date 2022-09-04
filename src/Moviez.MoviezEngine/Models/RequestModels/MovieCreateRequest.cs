using System;
namespace Moviez.MoviezEngine.Models.RequestModels
{
    public class MovieCreateRequest
    {
        
        public string? MovieName { get; set; }
        public DateTime DateOfRelease { get; set; }
        public long? ProducerId { get; set; }
        public string? PosterLink { get; set; }
        public string? Plot { get; set; }

        public List<long>? ActorRequestIds { get; set; }
    }
}

