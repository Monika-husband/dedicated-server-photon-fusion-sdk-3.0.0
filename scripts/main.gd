extends Node3D

const CharacterScene = preload("res://scenes/authority_character_3d.tscn")

@onready var spawner: FusionSpawner = $FusionSpawner
@onready var lobby_ui: Control = $lobby

var _player_characters: Dictionary = {}

func _ready():
	if _is_dedicated_server():
		print("[SERVER] Dedicated Server active.")
		lobby_ui.queue_free()
		_start_dedicated_server()
	else:
		print("[CLIENT] Client mode active.")


func _start_dedicated_server():
	var room_name = OS.get_environment("ROOM_NAME")
	if room_name.is_empty():
		room_name = "VPS_Server_%d" % randi_range(100, 999)
	
	Fusion.connect_to_photon.call_deferred("server_" + room_name, "eu")
	
	Fusion.connected_to_photon.connect(func():
		var opts = FusionRoomOptions.new()
		opts.is_visible = true
		opts.max_players = 16
		Fusion.join_or_create_room(room_name, opts)
	)

	Fusion.room_joined.connect(_on_server_room_joined)
	Fusion.player_joined.connect(_on_player_joined)
	Fusion.player_left.connect(_on_player_left)
func _on_server_room_joined():
	print("[SERVER] Room registered on Photon: ", Fusion.get_room().room_name)
func _is_dedicated_server() -> bool:
	return DisplayServer.get_name() == "headless" or OS.has_feature("dedicated_server")

func _on_player_joined(player, extra_arg):
	print("Подключился участник с ID: ", player)
	if not Fusion.is_master_client():
		return
	if _is_dedicated_server() and player == 1:
		return
	if _player_characters.has(player):
		return
	_spawn_character(player)
	call_deferred("_catch_up_nicknames", player)

func _on_player_left(player_id, extra_arg):
	print("Игрок вышел: ", player_id)

	if player_id == 1:
		_end_session_for_everyone()
		return

	if not Fusion.is_master_client():
		return
	if _player_characters.has(player_id):
		var char_node = _player_characters[player_id]
		if is_instance_valid(char_node):
			spawner.despawn(char_node)
		_player_characters.erase(player_id)
		print("Персонаж игрока ", player_id, " нахуй пошол")

func _end_session_for_everyone():
	if OS.has_feature("dedicated_server"):
		return # сам сервер этого события получить не должен, но на всякий случай

	print("Хост/сервер покинул комнату — возвращаем всех в лобби.")

	for pid in _player_characters.keys():
		var char_node = _player_characters[pid]
		if is_instance_valid(char_node):
			char_node.queue_free()
	_player_characters.clear()

	var local_character = get_tree().get_first_node_in_group("local_player")
	if is_instance_valid(local_character):
		local_character.queue_free()

	var tab_players = get_node_or_null("TabPlayers")
	if tab_players:
		if "_tabs" in tab_players:
			for character in tab_players._tabs.keys():
				if is_instance_valid(tab_players._tabs[character]):
					tab_players._tabs[character].queue_free()
			tab_players._tabs.clear()
		if tab_players.has_method("hide_list"):
			tab_players.hide_list()

	if is_instance_valid(lobby_ui):
		lobby_ui.show()

	Fusion.leave_room()

func _catch_up_nicknames(new_player_id: int):
	for pid in _player_characters:
		if pid == new_player_id:
			continue
		var char_node = _player_characters[pid]
		if is_instance_valid(char_node) and char_node.player_nickname != "":
			# таргетимся именно на нового игрока, но RPC всё равно идёт
			# через сетевой объект char_node, а не через Main
			Fusion.rpc_to_player(new_player_id, char_node.apply_nickname, char_node.player_nickname, char_node.player_steam_id)


func _spawn_character(player_id: int):
	var character = spawner.spawn()
	character.position = Vector3(randf_range(-4, 4), 1.0, randf_range(-4, 4))
	var replicator = character.get_node("FusionServerReplicator")
	replicator.set_input_authority(player_id)
	_player_characters[player_id] = character
