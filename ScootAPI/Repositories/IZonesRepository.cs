using ScootAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScootAPI.Repositories
{
    public interface IZonesRepository
    {
        Task AddZone(Zone zone);

        Task UpdateZone(Zone zone);

        Task DeleteZone(string id);

        Task<Zone> GetZone(string id);

        Task<List<Zone>> GetZones();
    }
}
