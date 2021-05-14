using System.ComponentModel.DataAnnotations.Schema;

namespace ScootAPI.Models
{
    public class Scooter
    {
        [Column(TypeName = "jsonb")]
        public Customer Location { get; set; }
        public bool IsDisabled { get; set; }
        public string Name { get; set; }
        public string IdZone { get; set; }
        public string IdScooter { get; set; }

    }

    public class Customer
    {
        public string Longitude { get; set; }
        public string Latitude { get; set; }
}
}
