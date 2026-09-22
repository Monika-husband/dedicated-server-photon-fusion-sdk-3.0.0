extends CharacterBody3D

const WALK_SPEED := 4.0
const SPRINT_SPEED := 7.0
const CROUCH_SPEED := 2.0
const JUMP_VELOCITY := 4.5
const GRAVITY := 20.0

const MOUSE_SENSITIVITY := 0.002
const BOB_FREQ := 2.5
const BOB_AMP := 0.08
var _t_bob := 0.0
var _pitch := 0.0

@onready var replicator: FusionServerReplicator = $FusionServerReplicator
@onready var head: Node3D = $Head
@onready var camera: Camera3D = $Head/Camera3D
@onready var collision_shape: CollisionShape3D = $CollisionShape3D
@onready var stand_raycast: RayCast3D = $Head/StandRayCast
@onready var player_list: Control = get_node_or_null("../TabPlayers")
var _tick: int = 0
var _pending_yaw_delta := 0.0
var player_nickname:String
var player_steam_id: int = 0
var _local_setup_done := false

func _announce_nickname():
	if Fusion.is_master_client():
		register_nickname(PlayerData.nickname, PlayerData.steam_id)
	else:
		Fusion.rpc_to(Fusion.RpcTarget.TARGET_MASTER, register_nickname, PlayerData.nickname, PlayerData.steam_id)
@rpc("any_peer", "call_local", "reliable")
func register_nickname(nick: String, steam_id: int):
	if not Fusion.is_master_client():
		return
	set_nickname(nick, steam_id) # обновляем сразу и у себя, не ждём эха от TARGET_ALL
	Fusion.rpc_to(Fusion.RpcTarget.TARGET_ALL, apply_nickname, nick, steam_id)

@rpc("any_peer", "call_local", "reliable")
func apply_nickname(nick: String, steam_id: int):
	set_nickname(nick, steam_id)

func set_nickname(nick: String, steam_id: int) -> void:
	player_nickname = nick
	player_steam_id = steam_id
	print("[NICK] set_nickname called: nick=", nick, " steam_id=", steam_id, " player_list=", player_list)
	if has_node("NicknameLabel"):
		$NicknameLabel.text = nick
	if player_list:
		player_list.register_player(self, nick, steam_id)
	else:
		print("[NICK] player_list is NULL! path was: ../TabPlayers, self path: ", get_path())
	

func _ready():
	if replicator and replicator.has_signal("on_process_input"):
		if not replicator.on_process_input.is_connected(process_input):
			replicator.on_process_input.connect(process_input)
	if player_list:
		player_list.register_player(self, player_nickname)

func _unhandled_input(event):
	if not replicator.has_input_authority(): return
	if event is InputEventMouseMotion and Input.get_mouse_mode() == Input.MOUSE_MODE_CAPTURED:
		_pending_yaw_delta += -event.relative.x * MOUSE_SENSITIVITY
		_pitch = clamp(_pitch - event.relative.y * MOUSE_SENSITIVITY, -PI/2, PI/2)
		head.rotation.x = _pitch

func _create_input() -> PackedByteArray:
	var buf = PackedByteArray()
	buf.resize(24)
	
	var input_dir = Input.get_vector("ui_left", "ui_right", "ui_up", "ui_down")
	var direction = (transform.basis * Vector3(input_dir.x, 0, input_dir.y)).normalized()
	
	var flags: int = 0	
	if Input.is_action_pressed("jump"): flags |= 1
	if Input.is_action_pressed("sprint"): flags |= 2
	
	var wants_to_crouch = Input.is_action_pressed("crouch")
	var is_blocked_above = stand_raycast.is_colliding()
	if wants_to_crouch or is_blocked_above:
		flags |= 4
	
	buf.encode_float(0, direction.x)
	buf.encode_float(4, direction.z)
	buf.encode_u32(8, flags)
	buf.encode_u32(12, _tick)	
	buf.encode_float(16, _pending_yaw_delta)
	buf.encode_float(20, _pitch)
	_pending_yaw_delta = 0.0
	_tick += 1
	
	return buf

func _physics_process(delta):
	if replicator.has_input_authority():
		replicator.queue_input(delta, _create_input())
	
	replicator.process_input_queue(delta)

func process_input(tick: int, delta_time: float, payload: PackedByteArray, is_new: bool):
	var dir_x = payload.decode_float(0)
	var dir_z = payload.decode_float(4)
	var flags = payload.decode_u32(8)
	var is_jumping = (flags & 1) != 0
	var is_sprinting = (flags & 2) != 0
	var yaw_delta = payload.decode_float(16)
	var pitch = payload.decode_float(20)
	rotate_y(yaw_delta)
	
	var direction = Vector3(dir_x, 0, dir_z)
	head.rotation.x = pitch
	
	var target_speed = WALK_SPEED
	if is_sprinting:
		if is_sprinting: target_speed = SPRINT_SPEED
	if not is_on_floor():
		velocity.y -= GRAVITY * delta_time
	else:
		if is_jumping:
			velocity.y = JUMP_VELOCITY
		else:
			velocity.y = -0.1

	if direction:
		velocity.x = lerp(velocity.x, direction.x * target_speed, 10.0 * delta_time)
		velocity.z = lerp(velocity.z, direction.z * target_speed, 10.0 * delta_time)
	else:
		velocity.x = lerp(velocity.x, 0.0, 10.0 * delta_time)
		velocity.z = lerp(velocity.z, 0.0, 10.0 * delta_time)

	move_and_slide()

func _process(delta):
	if not _local_setup_done and replicator.has_input_authority():
		_local_setup_done = true
		Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
		add_to_group("local_player")
		_announce_nickname()

	if replicator.has_input_authority():
		if not camera.current:
			camera.make_current()
			Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
		if is_on_floor() and velocity.length() > 1.0:
			_t_bob += delta * velocity.length() * float(is_on_floor())
			var bob_y = sin(_t_bob * BOB_FREQ) * BOB_AMP
			var bob_x = cos(_t_bob * BOB_FREQ / 2.0) * BOB_AMP
			camera.transform.origin = Vector3(bob_x, bob_y, 0)
		else:
			camera.transform.origin = camera.transform.origin.lerp(Vector3.ZERO, delta * 5.0)

		if Input.is_action_just_pressed("ui_cancel"):
			if Input.mouse_mode == Input.MOUSE_MODE_CAPTURED:
				Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
			else:
				Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
	else:
		if camera.current:
			camera.current = false

@rpc("any_peer", "call_local", "reliable")
func broadcast_chat_message(message: String):
	var chat_ui = get_node("../ChatUI")
	if chat_ui and chat_ui.has_method("append_message"):
		chat_ui.append_message(player_nickname, message)

func send_chat(text: String):
	if not replicator.has_input_authority():
		return
	Fusion.rpc_to(Fusion.RpcTarget.TARGET_ALL, broadcast_chat_message, text)
func _exit_tree():
	if player_list:
		player_list.unregister_player(self)
