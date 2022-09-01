using System;
using System.Collections.Generic;

namespace Moviez.MoviezEngine.Entities
{
    public partial class Actor
    {
        public long ActorId { get; set; }
        public string ActorName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Bio { get; set; }
    }
}
