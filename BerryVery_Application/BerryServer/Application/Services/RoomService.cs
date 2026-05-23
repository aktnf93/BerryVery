using BerryServer.Application.Repositories;
using BerryServer.Domain.Entities;
using BerryServer.Infrastructure.Data;
using BerryServer.Infrastructure.Network;
using NuGet.Protocol.Core.Types;

namespace BerryServer.Application.Services
{
    public class RoomService
    {
        protected readonly ILogger<RoomService> _logger;
        protected readonly RoomRepository _repository;
        protected readonly SocketConnection _socket;

        public RoomService(ILogger<RoomService> logger, RoomRepository repository, SocketConnection socket)
        {
            this._logger = logger;
            this._repository = repository;
            this._socket = socket;
        }

        public IAsyncEnumerable<Room> GetRooms(CancellationToken cancellationToken)
        {
            return this._repository.GetRooms(cancellationToken);
        }
    }
}
