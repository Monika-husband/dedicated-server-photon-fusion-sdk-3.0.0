using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace FusionGodot;

/// <summary>The current room's view (wraps native FusionRoom).</summary>
public sealed class FusionRoom : FusionWrapper
{
    public FusionRoom(GodotObject obj) : base(obj) { }

    public string RoomName => CallString(MethodName.GetRoomName);

    public int PlayerCount => CallInt(MethodName.GetPlayerCount);

    public int MaxPlayers
    {
        get => CallInt(MethodName.GetMaxPlayers);
        set => CallVoid(MethodName.SetMaxPlayers, value);
    }

    public bool IsOpen
    {
        get => CallBool(MethodName.IsOpen);
        set => CallVoid(MethodName.SetOpen, value);
    }

    public bool IsVisible
    {
        get => CallBool(MethodName.IsVisible);
        set => CallVoid(MethodName.SetVisible, value);
    }

    public int MasterClientId => CallInt(MethodName.GetMasterClientId);

    public bool IsMasterClient => CallBool(MethodName.GetIsMasterClient);

    public int PlayerTtlMs => CallInt(MethodName.GetPlayerTtlMs);

    public int EmptyRoomTtlMs => CallInt(MethodName.GetEmptyRoomTtlMs);

    public bool SuppressRoomEvents => CallBool(MethodName.GetSuppressRoomEvents);

    public bool PublishUserId => CallBool(MethodName.GetPublishUserId);

    public int DirectMode => CallInt(MethodName.GetDirectMode);

    public string[] Plugins => Call(MethodName.GetPlugins).AsStringArray();

    public Dictionary CustomProperties => Call(MethodName.GetCustomProperties).AsGodotDictionary();

    public List<FusionPlayer> Players => Call(MethodName.GetPlayers).ToWrappers(o => new FusionPlayer(o));

    public string[] ExpectedUsers
    {
        get => Call(MethodName.GetExpectedUsers).AsStringArray();
        set => CallVoid(MethodName.SetExpectedUsers, value);
    }

    public string[] LobbyProperties
    {
        get => Call(MethodName.GetLobbyProperties).AsStringArray();
        set => CallVoid(MethodName.SetLobbyProperties, value);
    }

    /// <summary>Reassign the master client (master only).</summary>
    public void SetMasterClient(int playerNumber) => CallVoid(MethodName.SetMasterClient, playerNumber);

    /// <summary>Set custom room properties. Returns false if the operation was rejected.</summary>
    public bool SetProperties(Dictionary dict) => CallBool(MethodName.SetProperties, dict);

    /// <summary>Compare-and-set custom room properties: applies only if current values match <paramref name="expected"/>.</summary>
    public bool SetPropertiesCas(Dictionary dict, Dictionary expected) => CallBool(MethodName.SetPropertiesCas, dict, expected);

    public bool RemoveProperties(string[] keys) => CallBool(MethodName.RemoveProperties, keys);

    public bool SetProperty(string key, Variant value) => CallBool(MethodName.SetProperty, key, value);

    // --- Cached native member names (GDExtension gets no GodotSharp-generated caches) ---

    /// <summary>Cached StringNames for this class's methods.</summary>
    public class MethodName : RefCounted.MethodName
    {
        public static readonly StringName GetRoomName = "get_room_name";
        public static readonly StringName GetPlayerCount = "get_player_count";
        public static readonly StringName GetIsMasterClient = "get_is_master_client";
        public static readonly StringName GetPlayerTtlMs = "get_player_ttl_ms";
        public static readonly StringName GetEmptyRoomTtlMs = "get_empty_room_ttl_ms";
        public static readonly StringName GetSuppressRoomEvents = "get_suppress_room_events";
        public static readonly StringName GetPublishUserId = "get_publish_user_id";
        public static readonly StringName GetDirectMode = "get_direct_mode";
        public static readonly StringName GetCustomProperties = "get_custom_properties";
        public static readonly StringName GetPlayers = "get_players";
        public static readonly StringName GetPlugins = "get_plugins";
        public static readonly StringName GetMaxPlayers = "get_max_players";
        public static readonly StringName SetMaxPlayers = "set_max_players";
        public static readonly StringName IsOpen = "is_open";
        public static readonly StringName SetOpen = "set_open";
        public static readonly StringName IsVisible = "is_visible";
        public static readonly StringName SetVisible = "set_visible";
        public static readonly StringName GetMasterClientId = "get_master_client_id";
        public static readonly StringName SetMasterClient = "set_master_client";
        public static readonly StringName GetExpectedUsers = "get_expected_users";
        public static readonly StringName SetExpectedUsers = "set_expected_users";
        public static readonly StringName GetLobbyProperties = "get_lobby_properties";
        public static readonly StringName SetLobbyProperties = "set_lobby_properties";
        public static readonly StringName SetProperties = "set_properties";
        public static readonly StringName SetPropertiesCas = "set_properties_cas";
        public static readonly StringName RemoveProperties = "remove_properties";
        public static readonly StringName SetProperty = "set_property";
    }

    /// <summary>Cached StringNames for this class's properties.</summary>
    public class PropertyName : RefCounted.PropertyName
    {
        public static readonly StringName RoomName = "room_name";
        public static readonly StringName PlayerCount = "player_count";
        public static readonly StringName MaxPlayers = "max_players";
        public static readonly StringName Open = "open";
        public static readonly StringName Visible = "visible";
        public static readonly StringName CustomProperties = "custom_properties";
        public static readonly StringName Players = "players";
        public static readonly StringName MasterClientId = "master_client_id";
        public static readonly StringName IsMasterClient = "is_master_client";
        public static readonly StringName PlayerTtlMs = "player_ttl_ms";
        public static readonly StringName EmptyRoomTtlMs = "empty_room_ttl_ms";
        public static readonly StringName ExpectedUsers = "expected_users";
        public static readonly StringName LobbyProperties = "lobby_properties";
        public static readonly StringName DirectMode = "direct_mode";
        public static readonly StringName SuppressRoomEvents = "suppress_room_events";
        public static readonly StringName PublishUserId = "publish_user_id";
        public static readonly StringName Plugins = "plugins";
    }

    public class SignalName : RefCounted.SignalName
    {
    }
}
