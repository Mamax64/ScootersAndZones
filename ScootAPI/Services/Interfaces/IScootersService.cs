using ScootAPI.Models;
using System.Collections.Generic;

namespace ScootAPI.Services
{
    public interface IScootersService
    {
        Scooter GetScooter(string id);

        void UpdateScooter(Scooter scooter);

        void DeleteScooter(string id);

        void AddScooter(Scooter scooter);

        List<Scooter> GetScooters();
    }
}
