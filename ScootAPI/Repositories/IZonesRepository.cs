using ScootAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScootAPI.Repositories
{
    public interface IZonesRepository
    {
        void AddZone(Zone zone);

        void UpdateZone(Zone zone);

        void DeleteZone(string id);

        Zone GetZone(string id);

        List<Zone> GetZones();
    }
}
