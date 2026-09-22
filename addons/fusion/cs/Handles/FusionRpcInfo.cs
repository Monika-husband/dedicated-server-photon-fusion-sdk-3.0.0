using Godot;

namespace FusionGodot;

#pragma warning disable CS8632

/// <summary>Context about the currently executing RPC (wraps native FusionRpcInfo).</summary>
/// <remarks>Valid only while handling an RPC; obtain via <see cref="Fusion.GetRpcInfo"/>.</remarks>
public sealed class FusionRpcInfo : FusionWrapper
{
    public FusionRpcInfo(GodotObject obj) : base(obj) { }

    /// <summary>Player id that sent the RPC.</summary>
    public int Sender => CallInt(MethodName.GetSender);

    /// <summary>Raw target value the RPC was sent to (an <see cref="RpcTarget"/> group value or a player id).</summary>
    public int Target => CallInt(MethodName.GetTarget);

    /// <summary>The node the RPC targets, if any.</summary>
    public Node? TargetNode => Call(MethodName.GetTargetNode).As<Node>();

    // --- Cached native member names (GDExtension gets no GodotSharp-generated caches) ---

    /// <summary>Cached StringNames for this class's methods.</summary>
    public class MethodName : RefCounted.MethodName
    {
        public static readonly StringName GetSender = "get_sender";
        public static readonly StringName GetTarget = "get_target";
        public static readonly StringName GetTargetNode = "get_target_node";
    }

    /// <summary>Cached StringNames for this class's properties.</summary>
    public class PropertyName : RefCounted.PropertyName
    {
        public static readonly StringName Sender = "sender";
        public static readonly StringName Target = "target";
        public static readonly StringName TargetNode = "target_node";
    }

    /// <summary>Cached StringNames for this class's signals.</summary>
    public class SignalName : RefCounted.SignalName
    {
    }
}
