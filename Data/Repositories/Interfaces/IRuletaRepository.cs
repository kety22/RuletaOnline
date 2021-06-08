using Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories.Interfaces
{
    public interface IRuletaRepository
    {
        Task<Roulette> GetRouletteById(string id);
        Task<Roulette> UpdateRoulette(Roulette roulette);
        Task<List<Roulette>> GetRoulettes();
    }
}
