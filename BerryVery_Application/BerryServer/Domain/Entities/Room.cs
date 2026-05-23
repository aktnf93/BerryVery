using System.Data;
using System.Data.Common;

namespace BerryServer.Domain.Entities
{
    public class Room : IMapper<Room>
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
        
        public bool IsDeleted { get; set; }

        public static Room Map(DbDataReader reader)
        {
            return new Room()
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Name = reader.GetString(reader.GetOrdinal("name")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                UpdatedAt = reader.GetDateTime(reader.GetOrdinal("updated_at")),
                IsDeleted = reader.GetBoolean(reader.GetOrdinal("is_deleted")),
            };
        }
    }
}
