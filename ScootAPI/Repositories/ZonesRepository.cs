using Microsoft.EntityFrameworkCore;
using ScootAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScootAPI.Repositories
{
    public class ZonesRepository : IZonesRepository
    {
        private readonly PostgreSQLContext _context;

        public ZonesRepository(PostgreSQLContext context)
        {
            _context = context;
        }

        public async Task AddZone(Zone zone)
        {
            _context.Zones.Add(zone);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateZone(Zone zone)
        {
            _context.Zones.Update(zone);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteZone(string id)
        {
            var entity = _context.Zones.FirstOrDefault(t => t.IdZone == id);
            _context.Zones.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<Zone> GetZone(string id)
        {
            return await _context.Zones.FirstOrDefaultAsync(t => t.IdZone == id);
        }

        public async Task<List<Zone>> GetZones()
        {
            return await _context.Zones.ToListAsync();
        }
    }
}
