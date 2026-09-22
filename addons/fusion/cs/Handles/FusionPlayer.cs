using Godot;

namespace FusionGodot;

/// <summary>A read-only snapshot of a single player in the connected room (wraps native FusionPlayer).</summary>
/// <remarks>
/// This is a copy taken at the time of creation; it does not track live data.
/// Re-query <see cref="FusionRoom.Players"/> to get up-to-date values.
/// </remarks>
public sealed class FusionPlayer : FusionWrapper
{
    public FusionPlayer(GodotObject obj) : base(obj) { }

    /// <summary>The player's number within the room (its unique identifier among the room's players).</summary>
    public int Number => CallInt(MethodName.GetNumber);

    /// <summary>The player's nickname.</summary>
    public string Name => CallString(MethodName.GetName);

    /// <summary>The player's user ID.</summary>
    public string UserId => CallString(MethodName.GetUserId);

    /// <summary>Whether the player is currently inactive (left, but still within the player TTL grace window).</summary>
    public bool IsInactive => CallBool(MethodName.GetIsInactive);

    /// <summary>Whether the player is the room's master client.</summary>
    public bool IsMasterClient => CallBool(MethodName.GetIsMasterClient);

    // --- Cached native member names (GDExtension gets no GodotSharp-generated caches) ---

    /// <summary>Cached StringNames for this class's methods.</summary>
    public class MethodName : RefCounted.MethodName
    {
        public static readonly StringName GetNumber = "get_number";
        public static readonly StringName GetName = "get_name";
        public static readonly StringName GetUserId = "get_user_id";
        public static readonly StringName GetIsInactive = "get_is_inactive";
        public static readonly StringName GetIsMasterClient = "get_is_master_client";
    }

    /// <summary>Cached StringNames for this class's properties.</summary>
    public class PropertyName : RefCounted.PropertyName
    {
        public static readonly StringName Number = "number";
        public static readonly StringName Name = "name";
        public static readonly StringName UserId = "user_id";
        public static readonly StringName IsInactive = "is_inactive";
        public static readonly StringName IsMasterClient = "is_master_client";
    }

    /// <summary>Cached StringNames for this class's signals.</summary>
    public class SignalName : RefCounted.SignalName
    {
    }
}
