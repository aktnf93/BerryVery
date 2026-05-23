using BerryServer.Domain.Entities;
using BerryServer.Infrastructure.Data;
using MySql.Data.MySqlClient;
using NuGet.Protocol.Core.Types;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace BerryServer.Application.Repositories
{
    public class RoomRepository
    {
        private readonly ILogger<RoomRepository> _logger;
        private readonly DatabaseConnection _db;

        public RoomRepository(ILogger<RoomRepository> logger, DatabaseConnection db)
        {
            this._logger = logger;
            this._db = db;
        }

        //public IAsyncEnumerable<Room> GetRooms(CancellationToken cancellationToken)
        //{
        //    return this._db.GetCommandAsync<Room>("SELECT * FROM tb_room", null, cancellationToken);
        //}

        // IAsyncEnumerable 내부에서 await를 쓰기 위해 메서드에 async를 붙여줍니다.
        public async IAsyncEnumerable<Room> GetRooms([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            // 1. 성능 측정을 위한 타이머 및 카운터 시작
            var stopwatch = Stopwatch.StartNew();
            long rowCount = 0;

            _logger.LogInformation("데이터 스트리밍 조회를 시작합니다.");

            IAsyncEnumerable<Room> stream;
            try
            {
                stream = _db.GetCommandAsync<Room>("SELECT * FROM tb_room", null, cancellationToken);
            }
            catch (Exception ex)
            {
                // DB 연결 자체가 실패했을 때의 로그
                _logger.LogError(ex, "데이터 스트리밍 초기화 중 예외가 발생했습니다.");
                throw;
            }

            // 2. 스트림을 소비하면서 카운트만 체크 (로그는 찍지 않음)
            await foreach (var room in stream.WithCancellation(cancellationToken))
            {
                rowCount++;
                yield return room;
            }

            stopwatch.Stop();

            // 3. 정상 종료 로그 (성능 모니터링용 데이터 수집)
            _logger.LogInformation(
                "데이터 스트리밍 완료. 총 건수: {RowCount}건, 소요 시간: {ElapsedMs}ms, 평균 처리 속도: {Speed}건/sec",
                rowCount,
                stopwatch.ElapsedMilliseconds,
                rowCount / Math.Max(1, stopwatch.Elapsed.TotalSeconds));
        }
    }
}
