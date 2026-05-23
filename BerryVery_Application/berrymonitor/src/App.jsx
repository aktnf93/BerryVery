import { useState, useEffect } from 'react';

function App() {

    const [rooms, setRooms] = useState([]);
    const [loading, setLoading] = useState(false);

    const fetchRoomsStream = async () => {
        setLoading(true);
        setRooms([]); // 이전 데이터 초기화

        try {
            // 💡 [핵심] axios 대신 브라우저 기본 fetch를 사용해 스트림을 엽니다.
            const response = await fetch("http://localhost:8016/api/room");

            const reader = response.body.getReader();
            const decoder = new TextDecoder("utf-8");
            let buffer = "";

            while (true) {
                const { done, value } = await reader.read();
                if (done) break;

                // 1. 들어온 청크 바이트를 텍스트로 변환하여 버퍼에 누적
                buffer += decoder.decode(value, { stream: true });

                // 2. 💡 [핵심 해결책] 정규식을 사용해 버퍼 안에서 완성된 형태의 { ... } 객체들만 쏙쏙 추출합니다.
                // 이 방식은 앞뒤에 붙은 대괄호([, ])나 쉼표(,)에 영향을 전혀 받지 않습니다.
                const jsonObjectRegex = /\{[^{}]*\}/g;
                let match;
                let lastIndex = 0;

                // 현재 버퍼에서 매칭되는 모든 완성된 JSON 객체를 찾아 루프를 돕니다.
                while ((match = jsonObjectRegex.exec(buffer)) !== null) {
                    const jsonString = match[0];
                    lastIndex = jsonObjectRegex.lastIndex;

                    try {
                        // 완성된 객체이므로 안전하게 파싱 성공!
                        const roomObj = JSON.parse(jsonString);

                        // 화면 갱신을 위해 상태 누적
                        setRooms(prev => [...prev, roomObj]);
                    } catch (err) {
                        // 혹시 내부에 중첩 괄호가 있어서 임시 실패한 경우 무시하고 진행
                        console.error("JSON 파싱 실패 (무시):", err, "문자열:", jsonString);
                    }
                }

                // 3. 처리 완료된 객체들은 버퍼에서 잘라내고, 뒤에 남은 미완성 조각만 버퍼에 남겨둡니다.
                if (lastIndex > 0) {
                    buffer = buffer.substring(lastIndex);
                }
            }
        } catch (error) {
            console.error("대용량 스트리밍 수신 중 오류 발생:", error);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchRoomsStream();
    }, []);

    return (
        <div style={{ padding: '20px' }}>
            <h2>실시간 대용량 객실 목록 조회 시스템</h2>

            <button onClick={fetchRoomsStream} disabled={loading} style={{ marginBottom: '10px', padding: '8px 16px' }}>
                {loading ? '데이터 실시간 동기화 중...' : '다시 불러오기'}
            </button>

            <div style={{ marginBottom: '10px', fontWeight: 'bold' }}>
                현재 화면에 로드된 개수: <span style={{ color: 'blue' }}>{rooms.length}</span> 개
            </div>

            {/* 1,000만 건이 스크롤되도록 가벼운 레이아웃 구성 */}
            <div style={{ border: '1px solid #ccc', height: '500px', overflowY: 'scroll', padding: '10px' }}>
                {rooms.map((room, index) => (
                    <div key={index} style={{ padding: '8px', borderBottom: '1px solid #eee', display: 'flex', justifyContent: 'space-between' }}>
                        <span>🆔 {room.id}</span>
                        <span style={{ fontWeight: 'bold' }}>🚪 {room.name}</span>
                        <span style={{ color: '#666', fontSize: '12px' }}>🕒 {room.createdAt}</span>
                    </div>
                ))}
                {rooms.length === 0 && !loading && <p>데이터가 없습니다.</p>}
            </div>
        </div>
    );
}

export default App;

