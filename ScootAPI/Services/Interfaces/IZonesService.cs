using ScootAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScootAPI.Services
{
    public interface IZonesService
    {
        List<Scooter> GetScootersByZoneId(string id);

        Task AddZone(Zone zone);

        Task UpdateZone(Zone zone);

        Task DeleteZone(string id);

        Task<Zone> GetZone(string id);

        Task<List<Zone>> GetZones();
    }
}
