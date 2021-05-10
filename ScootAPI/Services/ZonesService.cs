using ScootAPI.Models;
using ScootAPI.Repositories;
using System.Collections.Generic;
using System.Linq;

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

            filteredScooters = _scootersRepository.GetScooters().Where(s => s.IdZone == id).ToList();

            return filteredScooters;
        }

        public void AddZone(Zone zone)
        {
            _zonesRepository.AddZone(zone);
        }

        public void UpdateZone(Zone zone)
        {
            _zonesRepository.UpdateZone(zone);
        }

        public void DeleteZone(string id)
        {
            _zonesRepository.DeleteZone(id);
        }

        public Zone GetZone(string id)
        {
            return _zonesRepository.GetZone(id);
        }

        public List<Zone> GetZones()
        {
            return _zonesRepository.GetZones();
        }
    }
}
