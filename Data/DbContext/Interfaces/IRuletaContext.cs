using StackExchange.Redis;

namespace Data.DbContext.Interfaces
{
    public interface IRuletaContext
    {
        IDatabase RedisDatabase { get; }
        IServer RedisServer { get; }
    }
}
