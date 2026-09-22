using System;
using Godot;

namespace FusionGodot;

/// <summary>
/// Concrete replicator for client-server topology with input prediction.
/// </summary>
public sealed class FusionServerReplicator : FusionReplicator
{
    public FusionServerReplicator(GodotObject obj) : base(obj) { }

    /// <summary>True when this client holds input authority over the object.</summary>
    public bool HasInputAuthority() => CallBool(MethodName.HasInputAuthority);

    public int InputAuthority
    {
        get => CallInt(MethodName.GetInputAuthority);
        set => CallVoid(MethodName.SetInputAuthority, value);
    }

    public int GetInputQueueCount() => CallInt(MethodName.GetInputQueueCount);

    public float GetInputQueueDeltaTime() => CallFloat(MethodName.GetInputQueueDeltaTime);

    /// <summary>Enqueue an input payload for the given delta time.</summary>
    public void QueueInput(float deltaTime, byte[] payload) => CallVoid(MethodName.QueueInput, deltaTime, payload);

    public void ProcessInputQueue(float deltaTime) => CallVoid(MethodName.ProcessInputQueue, deltaTime);

    /// <summary>
    /// Fires when the server processes an input tick. Handler receives
    /// (tick, deltaTime, payload, isNew).
    /// </summary>
    public event Action<int, float, byte[], bool> OnProcessInput
    {
        add => Obj.Connect(SignalName.OnProcessInput, Callable.From(value));
        remove => Obj.Disconnect(SignalName.OnProcessInput, Callable.From(value));
    }

    // --- Cached native member names (GDExtension gets no GodotSharp-generated caches) ---

    /// <summary>Cached StringNames for this class's methods.</summary>
    public new class MethodName : FusionReplicator.MethodName
    {
        public static readonly StringName HasInputAuthority = "has_input_authority";
        public static readonly StringName GetInputAuthority = "get_input_authority";
        public static readonly StringName SetInputAuthority = "set_input_authority";
        public static readonly StringName GetInputQueueCount = "get_input_queue_count";
        public static readonly StringName GetInputQueueDeltaTime = "get_input_queue_delta_time";
        public static readonly StringName QueueInput = "queue_input";
        public static readonly StringName ProcessInputQueue = "process_input_queue";
    }

    /// <summary>Cached StringNames for this class's properties.</summary>
    public new class PropertyName : FusionReplicator.PropertyName
    {
        public static readonly StringName OwnerMode = "owner_mode";
    }

    /// <summary>Cached StringNames for this class's signals.</summary>
    public new class SignalName : FusionReplicator.SignalName
    {
        public static readonly StringName OnProcessInput = "on_process_input";
    }
}
