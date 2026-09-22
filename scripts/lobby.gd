extends Control

@onready var room_list: ItemList = $RoomList
@onready var refresh_btn: Button = $RefreshButton
@onready var join_btn: Button = $JoinButton
@onready var create_private_btn: Button = $CreatePrivateButton
@onready var password_input: LineEdit = $PasswordInput
@onready var status_label: Label = $StatusLabel
@onready var nickname_input: LineEdit = $NicknameInput

var _rooms: Array[FusionRoomListing] = []
var _pending_password: String = ""

func _ready():
	# игнор инициализацию интерфейса на хеадлесс-сервере
	if DisplayServer.get_name() == "headless" or OS.has_feature("dedicated_server"):
		return

	Fusion.connect_to_photon.call_deferred("player_%d" % randi())
	Fusion.connected_to_photon.connect(_on_connected_to_photon)
	Fusion.room_list_updated.connect(_on_room_list_updated)
	Fusion.room_joined.connect(_on_room_joined)
	nickname_input.text = PlayerData.nickname
	nickname_input.text_changed.connect(func(t): PlayerData.nickname = t)
	refresh_btn.pressed.connect(_refresh_room_list)
	join_btn.pressed.connect(_join_selected_room)
	create_private_btn.pressed.connect(_create_private_room)

func _on_connected_to_photon():
	status_label.text = "LOGS: Подключено. Загрузка серверов..."

func _on_room_list_updated(rooms: Array[FusionRoomListing]):
	_rooms = rooms
	_update_room_list_ui()

func _update_room_list_ui():
	room_list.clear()
	if _rooms.is_empty():
		status_label.text = "LOGS: Публичных серверов нет. Создайте приватную игру."
		return
	
	status_label.text = "LOGS: Найдено серверов: %d" % _rooms.size()
	
	for room in _rooms:
		var label = "%s [%d/%d]" % [
			room.name,
			room.player_count,
			room.max_players
		]
		
		if room.custom_properties.get("has_password", false):
			label += "нахуй пошол"
		
		room_list.add_item(label)

func _refresh_room_list():
	Fusion.connect_to_photon.call_deferred("player_%d" % randi())

func _join_selected_room():
	var selected = room_list.get_selected_items()
	if selected.is_empty():
		status_label.text = "LOGS: Выберите сервер!"
		return
	
	var room = _rooms[selected[0]]
	
	Fusion.join_room(room.name)

func _create_private_room():
	var room_name = password_input.text.strip_edges()
	if room_name.is_empty():
		status_label.text = "LOGS: Укажите код/название приватной комнаты!"
		return

	var opts = FusionRoomOptions.new()
	opts.is_visible = false
	opts.max_players = 8

	Fusion.join_or_create_room(room_name, opts)
func _on_room_joined():
	hide()

	var main_node = get_parent()
	var tab_players = main_node.get_node_or_null("TabPlayers")
	if tab_players and tab_players.has_method("show_list"):
		tab_players.show_list()

	if not Fusion.player_joined.is_connected(main_node._on_player_joined):
		Fusion.player_joined.connect(main_node._on_player_joined)
		Fusion.player_left.connect(main_node._on_player_left)
	if Fusion.is_master_client() and main_node.has_method("_spawn_character"):
		call_deferred("_ensure_self_spawned", main_node)

func _ensure_self_spawned(main_node: Node) -> void:
	var my_id = Fusion.get_local_player_id()
	if not main_node._player_characters.has(my_id):
		print("[FALLBACK] player_joined для себя не долетел вовремя, спавнимся вручную")
		main_node._spawn_character(my_id)

@rpc("any_peer", "call_local")
func force_leave_room():
	print("Исключение: неверный пароль.")
	Fusion.leave_room()
	show()
	status_label.text = "LOGS: Неверный пароль!"


func _on_tg_pressed() -> void:
	OS.shell_open("https://t.me/JackiSteam")
