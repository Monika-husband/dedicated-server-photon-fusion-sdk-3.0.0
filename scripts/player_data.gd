extends Node

var nickname: String = ""
var steam_id: int = 0

func _ready() -> void:
	if OS.has_feature("dedicated_server") or DisplayServer.get_name() == "headless":
		nickname = "Server"
		return
	if Engine.has_singleton("Steam") and Steam.isSteamRunning():
		steam_id = Steam.getSteamID()
		var steam_name = Steam.getPersonaName()
		nickname = steam_name if steam_name != "" else "Player_%d" % randi_range(1000, 9999)
	else:
		nickname = "Player_%d" % randi_range(1000, 9999)
