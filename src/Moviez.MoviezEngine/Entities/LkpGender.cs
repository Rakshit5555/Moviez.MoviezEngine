using System;
using System.Collections.Generic;

namespace Moviez.MoviezEngine.Entities
{
    public partial class LkpGender
    {
        public LkpGender()
        {
            Actors = new HashSet<Actor>();
            Producers = new HashSet<Producer>();
        }

        public string GenderCode { get; set; }
        public string GenderDesc { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Actor> Actors { get; set; }
        public virtual ICollection<Producer> Producers { get; set; }
    }
}
