using ScootAPI.Models;
using ScootAPI.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScootAPI.Services
{
    public class ScootersService : IScootersService
    {
        private readonly IScootersRepository _scooterRepository;
        public ScootersService(IScootersRepository scooterRepository)
        {
            _scooterRepository = scooterRepository;
        }

        public async Task<Scooter> GetScooter(string id)
        {
            return await _scooterRepository.GetScooter(id);
        }

        public async Task UpdateScooter(Scooter scooter)
        {
            await _scooterRepository.UpdateScooter(scooter);
        }

        public async Task DeleteScooter(string id)
        {
            await _scooterRepository.DeleteScooterAsync(id);
        }

        public async Task AddScooter(Scooter scooter)
        {
            await _scooterRepository.AddScooterAsync(scooter);
        }

        public async Task<List<Scooter>> GetScooters()
        {
            return await _scooterRepository.GetScooters();
        }
    }
}
