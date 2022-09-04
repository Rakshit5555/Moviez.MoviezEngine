using System;
using System.Collections.Generic;

namespace Moviez.MoviezEngine.Entities
{
    public partial class Actor
    {
        public Actor()
        {
            Castings = new HashSet<Casting>();
        }

        public long ActorId { get; set; }
        public string ActorName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string GenderCode { get; set; }
        public string Bio { get; set; }
        public bool IsActive { get; set; }

        public virtual LkpGender GenderCodeNavigation { get; set; }
        public virtual ICollection<Casting> Castings { get; set; }
    }
}
