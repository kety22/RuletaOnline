using Data.DbContext.Interfaces;
using StackExchange.Redis;

namespace Data.DbContext
{
    public class RuletaContext : IRuletaContext
    {

        #region Properties
        private readonly ConnectionMultiplexer _redisConnection;
        #endregion

        #region Constructor
        public RuletaContext(ConnectionMultiplexer redisConnection)
        {
            _redisConnection = redisConnection;
            RedisDatabase = redisConnection.GetDatabase();
            RedisServer = redisConnection.GetServer(redisConnection.Configuration);
        }
        #endregion

        #region Methods
        public IDatabase RedisDatabase { get; }
        public IServer RedisServer { get; }
        #endregion
    }
}
