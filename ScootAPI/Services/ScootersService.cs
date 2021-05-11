using ScootAPI.Models;
using ScootAPI.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace ScootAPI.Services
{
    public class ScootersService : IScootersService
    {
        private readonly IScootersRepository _scooterRepository;
        private readonly IAmqpService _amqpService;
        public ScootersService(IScootersRepository scooterRepository, IAmqpService amqpService)
        {
            _scooterRepository = scooterRepository;
            _amqpService = amqpService;
        }

        public Scooter GetScooter(string id)
        {
            return _scooterRepository.GetScooter(id);
        }

        public void UpdateScooter(Scooter scooter)
        {
            _scooterRepository.UpdateScooter(scooter);
        }

        public void DeleteScooter(string id)
        {
            _scooterRepository.DeleteScooter(id);
        }

        public void AddScooter(Scooter scooter)
        {
            _scooterRepository.AddScooter(scooter);
        }

        public List<Scooter> GetScooters()
        {
            return _scooterRepository.GetScooters();
        }
    }
}
