using System;
using System.Collections.Generic;

namespace Moviez.MoviezEngine.Entities
{
    public partial class Producer
    {
        public Producer()
        {
            Movies = new HashSet<Movie>();
        }

        public long ProducerId { get; set; }
        public string ProducerName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string GenderCode { get; set; }
        public string Company { get; set; }
        public string Bio { get; set; }
        public bool IsActive { get; set; }

        public virtual LkpGender GenderCodeNavigation { get; set; }
        public virtual ICollection<Movie> Movies { get; set; }
    }
}
