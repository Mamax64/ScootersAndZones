using System;
using System.Collections.Generic;

#nullable disable

namespace ScootAPI.Models
{
    public partial class Zone
    {
        public Zone()
        {
            Scooters = new HashSet<Scooter>();
        }

        public bool IsDisabled { get; set; }
        public string Name { get; set; }
        public string IdZone { get; set; }

        public virtual ICollection<Scooter> Scooters { get; set; }
    }
}
