using Data.DbContext.Interfaces;
using Data.Repositories.Interfaces;
using Entity;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class RuletaRepository : IRuletaRepository
    {
        #region Properties
        private readonly IRuletaContext _context;
        #endregion

        #region Constructor
        public RuletaRepository(IRuletaContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        #endregion

        #region Methods
        public async Task<Roulette> GetRouletteById(string id)
        {
            var roulette = await _context
                                .RedisDatabase
                                .StringGetAsync(id);
            if (roulette.IsNullOrEmpty)
                return null;

            return JsonConvert.DeserializeObject<Roulette>(roulette, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
        }

        public async Task<List<Roulette>> GetRoulettes()
        {
            var keys = GetKeys();
            List<Roulette> roulettes = new List<Roulette>();
            foreach (var key in keys)
                roulettes.Add(await GetRouletteById(key));

            return roulettes;
        }

        public IEnumerable<RedisKey> GetKeys()
        {
            return _context.RedisServer.Keys();
        }

        public async Task<Roulette> UpdateRoulette(Roulette roulette)
        {
            var updated = await _context
                              .RedisDatabase
                              .StringSetAsync(roulette.Id, JsonConvert.SerializeObject(roulette, Formatting.Indented,
                              new JsonSerializerSettings
                              {
                                  TypeNameHandling = TypeNameHandling.Objects
                              }));
            if (!updated)
                return null;

            return await GetRouletteById(roulette.Id);
        }

        #endregion
    }
}
