using ScootAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace ScootAPI.Repositories
{
    public class ZonesRepository : IZonesRepository
    {
        private readonly PostgreSQLContext _context;

        public ZonesRepository(PostgreSQLContext context)
        {
            _context = context;
        }

        public void AddZone(Zone zone)
        {
            _context.Zones.Add(zone);
            _context.SaveChanges();
        }

        public void UpdateZone(Zone zone)
        {
            _context.Zones.Update(zone);
            _context.SaveChanges();
        }

        public void DeleteZone(string id)
        {
            var entity = _context.Zones.FirstOrDefault(t => t.IdZone == id);
            _context.Zones.Remove(entity);
            _context.SaveChanges();
        }

        public Zone GetZone(string id)
        {
            return _context.Zones.FirstOrDefault(t => t.IdZone == id);
        }

        public List<Zone> GetZones()
        {
            return _context.Zones.ToList();
        }
    }
}
