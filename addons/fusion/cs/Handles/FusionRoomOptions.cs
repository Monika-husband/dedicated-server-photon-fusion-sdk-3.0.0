using Godot;
using Godot.Collections;

namespace FusionGodot;

/// <summary>Simple container class for Fusion's matchmaking arguments. (Wraps native FusionRoomOptions)</summary>
public sealed class FusionRoomOptions : FusionWrapper
{
    private static readonly StringName _className = "FusionRoomOptions";
    public FusionRoomOptions() : base(ClassDB.Instantiate(_className).AsGodotObject()) { }

    public bool IsOpen
    {
        get => CallBool(MethodName.GetIsOpen);
        set => Call(MethodName.SetIsOpen, value);
    }

    public bool IsVisible
    {
        get => CallBool(MethodName.GetIsVisible);
        set => Call(MethodName.SetIsVisible);
    }

    public byte MaxPlayers
    {
        get => (byte)CallInt(MethodName.GetMaxPlayers);
        set => Call(MethodName.SetMaxPlayers, value);
    }

    public int PlayerTtlMs
    {
        get => CallInt(MethodName.GetPlayerTtlMs);
        set => Call(MethodName.SetPlayerTtlMs, value);
    }

    public int EmptyRoomTtlMs
    {
        get => CallInt(MethodName.GetEmptyRoomTtlMs);
        set => Call(MethodName.SetEmptyRoomTtlMs, value);
    }

    public Dictionary CustomProperties
    {
        get => Call(MethodName.GetCustomProperties).AsGodotDictionary();
        set => Call(MethodName.SetCustomProperties, value);
    }

    public Array<string> LobbyProperties
    {
        get => Call(MethodName.GetLobbyProperties).AsGodotArray<string>();
        set => Call(MethodName.SetLobbyProperties, value);
    }

    public string LobbyName
    {
        get => CallString(MethodName.GetLobbyName);
        set => Call(MethodName.SetLobbyName, value);
    }

    public Array<string> Plugins
    {
        get => Call(MethodName.GetPlugins).AsGodotArray<string>();
        set => Call(MethodName.SetPlugins, value);
    }

    // --- Cached native member names (GDExtension gets no GodotSharp-generated caches) ---

    /// <summary>Cached StringNames for this class's methods.</summary>
    public class MethodName : RefCounted.MethodName
    {
        public static readonly StringName GetIsVisible = "get_is_visible";
        public static readonly StringName SetIsVisible = "set_is_visible";
        public static readonly StringName GetIsOpen = "get_is_open";
        public static readonly StringName SetIsOpen = "set_is_open";
        public static readonly StringName GetMaxPlayers = "get_max_players";
        public static readonly StringName SetMaxPlayers = "set_max_players";
        public static readonly StringName GetPlayerTtlMs = "get_player_ttl_ms";
        public static readonly StringName SetPlayerTtlMs = "set_player_ttl_ms";
        public static readonly StringName GetEmptyRoomTtlMs = "get_empty_room_ttl_ms";
        public static readonly StringName SetEmptyRoomTtlMs = "set_empty_room_ttl_ms";
        public static readonly StringName GetCustomProperties = "get_custom_properties";
        public static readonly StringName SetCustomProperties = "set_custom_properties";
        public static readonly StringName GetLobbyProperties = "get_lobby_properties";
        public static readonly StringName SetLobbyProperties = "set_lobby_properties";
        public static readonly StringName GetLobbyName = "get_lobby_name";
        public static readonly StringName SetLobbyName = "set_lobby_name";
        public static readonly StringName GetPlugins = "get_plugins";
        public static readonly StringName SetPlugins = "set_plugins";
    }

    /// <summary>Cached StringNames for this class's properties.</summary>
    public class PropertyName : RefCounted.PropertyName
    {
        public static readonly StringName IsVisible = "is_visible";
        public static readonly StringName IsOpen = "is_open";
        public static readonly StringName MaxPlayers = "max_players";
        public static readonly StringName PlayerTtlMs = "player_ttl_ms";
        public static readonly StringName EmptyRoomTtlMs = "empty_room_ttl_ms";
        public static readonly StringName CustomProperties = "custom_properties";
        public static readonly StringName LobbyProperties = "lobby_properties";
        public static readonly StringName LobbyName = "lobby_name";
        public static readonly StringName Plugins = "plugins";
    }

    /// <summary>Cached StringNames for this class's signals.</summary>
    public class SignalName : RefCounted.SignalName
    {
    }
}
