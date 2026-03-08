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


	Resources
	Views
	ViewModels
	Models
	Services
	Channels
	Domain
	Converters
	Common


{
	Resources
		Styles
		Images
	
	Views : 화면
		V_MainWindow
		
	ViewModels : 화면 처리 객체
		VM_MainWindow
		
	Models : 화면 데이터 모델
		M_RoomInfo
		M_RoomStatus
		
	Services : 통신
		S_SocketServer
		S_SocketClient
		S_WebSocketServer
		S_WebSocketClient
		S_Database
		S_HttpServer
		S_HttpClient
		S_SerialPort
		
	Channels : 통신 처리 객체
		C_Device
		C_Messenger
		C_Database
		C_Interface
		
	Domain : 통신 데이터 모델
		D_RoomData
		
	Converters : 데이터 변환
		Conv_Packet
		Conv_Query
		Conv_Message
		Conv_Model

	Common : 공통 로직
		Cm_Config
		Cm_Notify
		Cm_Logger
		Cm_Command
}
end