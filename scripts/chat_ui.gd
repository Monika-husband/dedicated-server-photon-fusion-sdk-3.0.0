extends Control

@onready var log: RichTextLabel = $ChatLog
@onready var input: LineEdit = $ChatInput

func _ready():
	input.text_submitted.connect(_on_submit)
	input.visible = false

func _unhandled_input(event):
	if event.is_action_pressed("chat_toggle"):
		input.visible = true
		input.grab_focus()
		Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
		get_viewport().set_input_as_handled()

func _on_submit(text: String):
	text = text.strip_edges()
	input.text = ""
	input.visible = false
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
	if text.is_empty():
		return
	var local_character = get_tree().get_first_node_in_group("local_player")
	if local_character and local_character.has_method("send_chat"):
		local_character.send_chat(text)
	else:
		push_warning("Локальный персонаж ещё не найден — сообщение не отправлено.")
func append_message(nickname: String, message: String) -> void:
	log.add_text("%s: " % nickname)
	log.push_color(Color.WHITE)
	log.add_text(message + "\n")
	log.pop()
