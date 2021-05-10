using System;
using System.Collections.Generic;

#nullable disable

namespace ScootAPI.Models
{
    public partial class Scooter
    {
        public string Location { get; set; }
        public bool IsDisabled { get; set; }
        public string Name { get; set; }
        public string IdZone { get; set; }
        public string IdScooter { get; set; }

        public virtual Zone IdZoneNavigation { get; set; }
    }
}
