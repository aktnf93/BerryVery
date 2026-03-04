BerryVery
- FMS 시설 관리 시스템
- 출입, 조명, 전열, 냉난방, 환기, 커튼


BerryDatabase
{

	berry_very

	H/W 정보
		- 통신 정보, 중계기 정보, 컨트롤러 정보, 디바이스 정보
		- id, name, type, address, status
		- tb_device_port
		- tb_device_gate
		- tb_device_ctrl
		- tb_device_sub
		- tb_device_type

	위치 정보
		- 빌딩 정보, 층 정보, 룸 정보
		- id, name
		- tb_zone_bldg
		- tb_zone_floor	| bldg_id
		- tb_zone_room	| floor_id

	모니터링 정보
		- 출입제어, 조명제어, 객실제어
		- id
		- tb_status_ctrl
		- tb_status_access
		- tb_status_light
		- tb_status_room
		- tb_status_temp

	이력
		- tb_log

	계정정보
		- 계정정보, 소속정보, 직급정보, 권한정보
		- tb_user_account	| dept_id | rank_id | role_id
		- tb_user_dept
		- tb_user_rank
		- tb_user_role
}
end

BerryDevice
BerrySchedule
BerryMessenger
BerryOperator
{
	Resources
		Styles
		Images
	
	Views : 화면
		V_...
		
	ViewModels : 화면 처리 객체
		VM_...
		
	Models : 화면 데이터 모델
		M_...
		
	Services : 통신
		S_SocketServer
		S_SocketClient
		S_WebSocketServer
		S_WebSocketClient
		S_Database
		S_HttpServer
		S_HttpClient
		
	Channels : 통신 처리 객체
		ChannelDevice
		ChannelMessenger
		ChannelDatabase
		ChannelInterface
		
	Domain : 통신 데이터 모델
		D_...
		
	Converters : 데이터 변환
		Converters
			ConvPacket
			ConvQuery
			ConvMessage
			ConvModel

	Common : 공통 로직
		Config
		Notify
		Logger
		Command
}
end