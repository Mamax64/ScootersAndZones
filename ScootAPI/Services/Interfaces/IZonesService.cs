using ScootAPI.Models;
using System.Collections.Generic;

namespace ScootAPI.Services
{
    public interface IZonesService
    {
        List<Scooter> GetScootersByZoneId(string id);

        void AddZone(Zone zone);

        void UpdateZone(Zone zone);

        void DeleteZone(string id);

        Zone GetZone(string id);

        List<Zone> GetZones();
    }
}
