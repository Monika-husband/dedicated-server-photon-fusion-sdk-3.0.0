extends Control
@onready var name_: Label = $name
@onready var avatar: TextureRect = $avatar

var _steam_id: int = 0

func _ready() -> void:
	print("[TAB] player_tab _ready, avatar node exists: ", has_node("avatar"))
	if Engine.has_singleton("Steam"):
		if not Steam.avatar_loaded.is_connected(_on_avatar_loaded):
			Steam.avatar_loaded.connect(_on_avatar_loaded)

func update_name(nickname: String) -> void:
	name_.text = nickname if nickname != "" else "..."

func update_avatar(steam_id: int) -> void:
	_steam_id = steam_id
	if steam_id == 0 or not Engine.has_singleton("Steam"):
		return
	Steam.getPlayerAvatar(Steam.AVATAR_MEDIUM, steam_id)

func _on_avatar_loaded(steam_id: int, size: int, buffer: PackedByteArray) -> void:
	if steam_id != _steam_id:
		return # аватар какого-то другого игрока, не наш
	var image = Image.create_from_data(size, size, false, Image.FORMAT_RGBA8, buffer)
	avatar.texture = ImageTexture.create_from_image(image)
