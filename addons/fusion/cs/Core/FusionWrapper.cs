using System;
using Godot;

namespace FusionGodot;

#pragma warning disable CS8632

/// <summary>
/// Base class for the typed C# views over Fusion's native GDExtension objects.
/// </summary>
/// <remarks>
/// This is a composition wrapper: it holds the underlying <see cref="GodotObject"/>
/// and forwards every operation through <see cref="GodotObject.Call(StringName, Variant[])"/>.
/// It uses only public GodotSharp API (no ptrcall, no instance binding), which is
/// why it can ship as portable source. The trade-off is boxed, name-dispatched
/// calls; if a faster path is needed later it can replace these helpers without
/// changing wrapper public surfaces.
/// </remarks>
public abstract partial class FusionWrapper
{
    /// <summary>The wrapped native object. Use as an escape hatch for unwrapped calls.</summary>
    protected internal readonly GodotObject Obj;

    protected FusionWrapper(GodotObject obj)
    {
        Obj = obj;
    }

    /// <summary>The underlying engine object (escape hatch).</summary>
    public GodotObject Self => Obj;

    /// <summary>True while the wrapped native instance is still alive.</summary>
    public bool IsValid => GodotObject.IsInstanceValid(Obj);

    // --- forwarding helpers (keep wrapper bodies one line each) ---

    protected Variant Call(StringName method, params Variant[] args) => Obj.Call(method, args);

    protected void CallVoid(StringName method, params Variant[] args) => Obj.Call(method, args);

    protected bool CallBool(StringName method, params Variant[] args) => Obj.Call(method, args).AsBool();

    protected int CallInt(StringName method, params Variant[] args) => Obj.Call(method, args).AsInt32();

    protected long CallLong(StringName method, params Variant[] args) => Obj.Call(method, args).AsInt64();

    protected float CallFloat(StringName method, params Variant[] args) => (float)Obj.Call(method, args).AsDouble();

    protected double CallDouble(StringName method, params Variant[] args) => Obj.Call(method, args).AsDouble();

    protected string CallString(StringName method, params Variant[] args) => Obj.Call(method, args).AsString();

    /// <summary>Connect a native signal. Returns the Callable so callers can Disconnect later.</summary>
    protected Callable ConnectSignal(StringName signal, Callable handler)
    {
        Obj.Connect(signal, handler);
        return handler;
    }

    // --- converting signal bridges (support typed C# events with arg conversion) ---

    private const string BridgeMetaPrefix = "__fusion_evt_";

    /// <summary>
    /// Holds a typed C# multicast delegate and the native <see cref="Callable"/> it is fed by.
    /// Anchored on the wrapped node's metadata so it is shared by every wrapper over that node
    /// (wrappers are throwaway views) and lives exactly as long as the node.
    /// </summary>
    private partial class SignalBridge : RefCounted
    {
        public Delegate? Handlers;
        public Callable Native;
    }

    private SignalBridge BridgeFor(StringName signal, Func<SignalBridge, Callable> connect)
    {
        string key = BridgeMetaPrefix + signal;
        if (Obj.HasMeta(key))
            return (SignalBridge)Obj.GetMeta(key).AsGodotObject();

        var bridge = new SignalBridge();
        bridge.Native = connect(bridge);   // build + connect the native-side Callable once
        Obj.SetMeta(key, bridge);          // node keeps the bridge (and its multicast) alive
        return bridge;
    }

    private void Retire(StringName signal, SignalBridge bridge)
    {
        if (bridge.Handlers is not null) return;   // still has subscribers
        Obj.Disconnect(signal, bridge.Native);
        Obj.RemoveMeta(BridgeMetaPrefix + signal);
    }

    /// <summary>
    /// Back a <c>event Action&lt;TDst&gt;</c> by a native single-arg signal whose argument
    /// (<typeparamref name="TSrc"/>) is converted to <typeparamref name="TDst"/>. Subscribe when
    /// <paramref name="subscribe"/> is true, unsubscribe otherwise. Works regardless of which
    /// wrapper instance the add/remove run on, and matches handlers by their own delegate identity.
    /// </summary>
    protected void ConvertedEvent<TSrc, TDst>(
        StringName signal, Func<TSrc, TDst> convert, Action<TDst> handler, bool subscribe)
    {
        if (!IsValid) return;   // node freed
        var bridge = BridgeFor(signal, b => ConnectSignal(signal, Callable.From(
            (TSrc src) => ((Action<TDst>?)b.Handlers)?.Invoke(convert(src)))));
        
        bridge.Handlers = subscribe
            ? (Action<TDst>?)bridge.Handlers + handler
            : (Action<TDst>?)bridge.Handlers - handler;
        Retire(signal, bridge);
    }
}
