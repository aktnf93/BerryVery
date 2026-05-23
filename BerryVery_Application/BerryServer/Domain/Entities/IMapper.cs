using System.Data.Common;

namespace BerryServer.Domain.Entities
{
    public interface IMapper<T> where T : class
    {
        public static abstract T Map(DbDataReader reader);
    }
}
