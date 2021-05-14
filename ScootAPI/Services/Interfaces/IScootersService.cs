using ScootAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScootAPI.Services
{
    public interface IScootersService
    {
        Task<Scooter> GetScooter(string id);

        Task UpdateScooter(Scooter scooter);

        Task DeleteScooter(string id);

        Task AddScooter(Scooter scooter);

        Task<List<Scooter>> GetScooters();
    }
}
