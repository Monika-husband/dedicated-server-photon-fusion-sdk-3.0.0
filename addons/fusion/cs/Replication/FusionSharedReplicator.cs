using System;
using Godot;

namespace FusionGodot;

/// <summary>
/// Concrete replicator for shared-authority topology (any client can request ownership).
/// </summary>
public sealed class FusionSharedReplicator : FusionReplicator
{
    public FusionSharedReplicator(GodotObject obj) : base(obj) { }

    /// <summary>
    /// Set or release this client's intent to own the object. When <paramref name="sendInterval"/>
    /// is greater than 0 it also adjusts the replicator's send tick interval.
    /// </summary>
    public void WantAuthority(bool want, int sendInterval = 0) => CallVoid(MethodName.WantAuthority, want, sendInterval);

    /// <summary>
    /// Fires on the owner when another client requests authority. The handler decides whether to
    /// grant it: return true to accept. (Native walks the connections and grants if any returns true.)
    /// </summary>
    public event Action<int, bool> AuthorityRequested
    {
        add => Obj.Connect(SignalName.AuthorityRequested, Callable.From(value));
        remove => Obj.Disconnect(SignalName.AuthorityRequested, Callable.From(value));
    }

    /// <summary>Fires on the requester with the outcome of a <see cref="WantAuthority"/> request.</summary>
    public event Action<bool> AuthorityResponse
    {
        add => Obj.Connect(SignalName.AuthorityResponse, Callable.From(value));
        remove => Obj.Disconnect(SignalName.AuthorityResponse, Callable.From(value));
    }

    // --- Cached native member names (GDExtension gets no GodotSharp-generated caches) ---

    /// <summary>Cached StringNames for this class's methods.</summary>
    public new class MethodName : FusionReplicator.MethodName
    {
        public static readonly StringName WantAuthority = "want_authority";
    }

    /// <summary>Cached StringNames for this class's properties.</summary>
    public new class PropertyName : FusionReplicator.PropertyName
    {
        public static readonly StringName OwnerMode = "owner_mode";
    }

    /// <summary>Cached StringNames for this class's signals.</summary>
    public new class SignalName : FusionReplicator.SignalName
    {
        public static readonly StringName AuthorityRequested = "authority_requested";
        public static readonly StringName AuthorityResponse = "authority_response";
    }
}
