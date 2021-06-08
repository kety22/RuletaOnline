using Entity.DTA;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IRuletaBusiness
    {
        Task<string> CreateRouletteAsync();
        Task<string> OpenRouletteByIdAsync(string id);
        Task<string> CreateBet(BetRequest betRequest);
        Task<RouletteResult> ClosedRouletteById(string id);
        Task<List<RouletteDetail>> GetRoulettes();
    }
}
