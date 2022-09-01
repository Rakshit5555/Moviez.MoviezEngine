using System;
using System.Collections.Generic;

namespace Moviez.MoviezEngine.Entities
{
    public partial class Movie
    {
        public long MovieId { get; set; }
        public string MovieName { get; set; }
        public DateOnly DateOfRelease { get; set; }
        public long? ProducerId { get; set; }
        public string PosterLink { get; set; }
        public string Plot { get; set; }

        public virtual Producer Producer { get; set; }
    }
}
