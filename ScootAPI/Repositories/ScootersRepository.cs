using Microsoft.EntityFrameworkCore;
using ScootAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScootAPI.Repositories
{
    public class ScootersRepository : IScootersRepository
    {
        private readonly PostgreSQLContext _context;

        public ScootersRepository(PostgreSQLContext context)
        {
            _context = context;
        }

        public async Task<Scooter> GetScooter(string id)
        {
            return await _context.Scooters.FirstOrDefaultAsync(t => t.IdScooter == id);
        }

        public async Task UpdateScooter(Scooter scooter)
        {
            _context.Scooters.Update(scooter);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteScooterAsync(string id)
        {
            var entity = _context.Scooters.FirstOrDefault(t => t.IdScooter == id);
            _context.Scooters.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AddScooterAsync(Scooter scooter)
        {
            _context.Scooters.Add(scooter);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Scooter>> GetScooters()
        {
            return await _context.Scooters.ToListAsync();
        }
    }
}
