extends Control

const PlayerTabScene = preload("res://scenes/player_tab.tscn")

@onready var rows_container: Control = $tab/container
@onready var leave_btn: Button = $leave

var _tabs: Dictionary = {} # character_node -> instance player_tab

func _ready() -> void:
	visible = false # скрыт, пока не зашли в комнату
	leave_btn.pressed.connect(_on_leave_pressed)

func show_list() -> void:
	visible = true

func hide_list() -> void:
	visible = false

func register_player(character: Node, nickname: String, steam_id: int = 0) -> void:
	print("[TABS] register_player called: nickname=", nickname, " steam_id=", steam_id)
	if _tabs.has(character):
		_tabs[character].update_name(nickname)
		_tabs[character].update_avatar(steam_id)
		return
	var tab = PlayerTabScene.instantiate()
	rows_container.add_child(tab)
	tab.update_name(nickname)
	tab.update_avatar(steam_id)
	_tabs[character] = tab
	print("[TABS] tab created, total tabs: ", _tabs.size())

func unregister_player(character: Node) -> void:
	if _tabs.has(character):
		_tabs[character].queue_free()
		_tabs.erase(character)

func _on_leave_pressed() -> void:
	# чистим локально всё, что видели в этой комнате
	for character in _tabs.keys():
		if is_instance_valid(_tabs[character]):
			_tabs[character].queue_free()
	_tabs.clear()

	var local_character = get_tree().get_first_node_in_group("local_player")
	if is_instance_valid(local_character):
		local_character.queue_free()

	hide_list()

	var lobby_ui = get_parent().get_node_or_null("lobby")
	if lobby_ui:
		lobby_ui.show()

	Fusion.leave_room()
