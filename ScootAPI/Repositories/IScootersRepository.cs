using ScootAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScootAPI.Repositories
{
    public interface IScootersRepository
    {
        Task<Scooter> GetScooter(string id);

        Task UpdateScooter(Scooter scooter);

        Task DeleteScooterAsync(string id);

        Task AddScooterAsync(Scooter scooter);

        Task<List<Scooter>> GetScooters();
    }
}
