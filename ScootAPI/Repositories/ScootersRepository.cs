using Npgsql;
using ScootAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace ScootAPI.Repositories
{
    public class ScootersRepository : IScootersRepository
    {
        private readonly PostgreSQLContext _context;

        public ScootersRepository(PostgreSQLContext context)
        {
            _context = context;
        }

        public Scooter GetScooter(string id)
        {
            return _context.Scooters.FirstOrDefault(t => t.IdScooter == id);
        }

        public void UpdateScooter(Scooter scooter)
        {
            _context.Scooters.Update(scooter);
            _context.SaveChanges();
        }

        public void DeleteScooter(string id)
        {
            var entity = _context.Scooters.FirstOrDefault(t => t.IdScooter == id);
            _context.Scooters.Remove(entity);
            _context.SaveChanges();
        }

        public void AddScooter(Scooter scooter)
        {
            _context.Scooters.Add(scooter);
            _context.SaveChanges();
        }

        public List<Scooter> GetScooters()
        {
            return _context.Scooters.ToList();
        }
    }
}
