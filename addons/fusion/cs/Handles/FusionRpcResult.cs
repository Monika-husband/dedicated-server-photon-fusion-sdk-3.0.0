using Godot;

namespace FusionGodot;

/// <summary>
/// Result handle returned by the <c>Fusion.Rpc*</c> calls (wraps native FusionRpcResult).
/// </summary>
public sealed class FusionRpcResult : FusionWrapper
{
    public FusionRpcResult(GodotObject obj) : base(obj) { }

    /// <summary>
    /// Register a callback invoked if the RPC fails within <paramref name="ttl"/> seconds.
    /// Returns this result for chaining.
    /// </summary>
    public FusionRpcResult OnFail(Callable callback, float ttl = 2.0f)
        => new(Call(MethodName.OnFail, callback, ttl).AsGodotObject());

    // --- Cached native member names (GDExtension gets no GodotSharp-generated caches) ---

    /// <summary>Cached StringNames for this class's methods.</summary>
    public class MethodName : RefCounted.MethodName
    {
        public static readonly StringName OnFail = "on_fail";
    }

    /// <summary>Cached StringNames for this class's properties.</summary>
    public class PropertyName : RefCounted.SignalName
    {
    }

    /// <summary>Cached StringNames for this class's signals.</summary>
    public class SignalName : RefCounted.SignalName
    {
    }
}
