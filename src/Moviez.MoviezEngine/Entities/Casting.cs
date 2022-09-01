using System;
using System.Collections.Generic;

namespace Moviez.MoviezEngine.Entities
{
    public partial class Casting
    {
        public long MovieId { get; set; }
        public long ActorId { get; set; }

        public virtual Actor Actor { get; set; }
        public virtual Movie Movie { get; set; }
    }
}
