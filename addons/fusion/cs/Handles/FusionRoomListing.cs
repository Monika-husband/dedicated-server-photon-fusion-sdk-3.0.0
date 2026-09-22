using Godot;
using Godot.Collections;

namespace FusionGodot;

/// <summary>One entry from the room list (wraps native FusionRoomListing).</summary>
/// <remarks>Obtained via <see cref="Fusion.GetRoomList"/> or the <see cref="Fusion.RoomListUpdated"/> signal.</remarks>
public sealed class FusionRoomListing : FusionWrapper
{
    public FusionRoomListing(GodotObject obj) : base(obj) { }

    public string Name => CallString(MethodName.GetName);

    public int PlayerCount => CallInt(MethodName.GetPlayerCount);

    public int MaxPlayers => CallInt(MethodName.GetMaxPlayers);

    public bool IsOpen => CallBool(MethodName.GetIsOpen);

    public int DirectMode => CallInt(MethodName.GetDirectMode);

    public Dictionary CustomProperties => Call(MethodName.GetCustomProperties).AsGodotDictionary();

    // --- Cached native member names (GDExtension gets no GodotSharp-generated caches) ---

    /// <summary>Cached StringNames for this class's methods.</summary>
    public class MethodName : RefCounted.MethodName
    {
        public static readonly StringName GetName = "get_name";
        public static readonly StringName GetPlayerCount = "get_player_count";
        public static readonly StringName GetMaxPlayers = "get_max_players";
        public static readonly StringName GetIsOpen = "get_is_open";
        public static readonly StringName GetDirectMode = "get_direct_mode";
        public static readonly StringName GetCustomProperties = "get_custom_properties";
    }

    /// <summary>Cached StringNames for this class's properties.</summary>
    public class PropertyName : RefCounted.PropertyName
    {
        public static readonly StringName Name = "name";
        public static readonly StringName PlayerCount = "player_count";
        public static readonly StringName MaxPlayers = "max_players";
        public static readonly StringName IsOpen = "is_open";
        public static readonly StringName DirectMode = "direct_mode";
        public static readonly StringName CustomProperties = "custom_properties";
    }

    /// <summary>Cached StringNames for this class's signals.</summary>
    public class SignalName : RefCounted.SignalName
    {
    }
}
