using System;
using Moviez.MoviezEngine.Entities;

namespace Moviez.MoviezEngine.Models.ResponseModels
{
    public class MovieDetailReponseForUser
    {
        
        public string MovieName { get; set; }
        public DateTime DateOfRelease { get; set; }
        public string ProducerName { get; set; }
        public string PosterLink { get; set; }
        public string Plot { get; set; }

        public List<ActorsResponse> Actors { get; set; }
    }

    public class MovieDetailReponse : MovieDetailReponseForUser
    {
        public long MovieId { get; set; }
        public long? ProducerId { get; set; }
        
    }
    public class ActorsResponse
    {

        public string ActorName { get; set; }

    }
}

