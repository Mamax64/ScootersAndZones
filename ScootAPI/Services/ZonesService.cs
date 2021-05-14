using ScootAPI.Models;
using ScootAPI.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScootAPI.Services
{
    public class ZonesService : IZonesService
    {
        private readonly IZonesRepository _zonesRepository;
        private readonly IScootersRepository _scootersRepository;
        public ZonesService(IZonesRepository zonesRepository, IScootersRepository scootersRepository)
        {
            _zonesRepository = zonesRepository;
            _scootersRepository = scootersRepository;
        }

        public List<Scooter> GetScootersByZoneId(string id)
        {
            List<Scooter> filteredScooters = new();

            filteredScooters = _scootersRepository.GetScooters().Result.Where(s => s.IdZone == id).ToList();

            return filteredScooters;
        }

        public async Task AddZone(Zone zone)
        {
            await _zonesRepository.AddZone(zone);
        }

        public async Task UpdateZone(Zone zone)
        {
            await _zonesRepository.UpdateZone(zone);
        }

        public async Task DeleteZone(string id)
        {
            await _zonesRepository.DeleteZone(id);
        }

        public async Task<Zone> GetZone(string id)
        {
            return await _zonesRepository.GetZone(id);
        }

        public async Task<List<Zone>> GetZones()
        {
            return await _zonesRepository.GetZones();
        }
    }
}
