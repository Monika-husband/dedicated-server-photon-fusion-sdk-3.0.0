using Godot;

namespace FusionGodot;

#pragma warning disable CS8632
    
/// <summary>
/// Typed view over a native FusionInterestArea node: defines an area-of-interest volume
/// (grid/circle/cone) that drives per-player interest management.
/// </summary>
public sealed class FusionInterestArea : FusionWrapper
{
    public FusionInterestArea(GodotObject obj) : base(obj) { }

    /// <summary>The wrapped node.</summary>
    public Node Node => (Node)Obj;

    public InterestShape Shape
    {
        get => (InterestShape)CallInt(MethodName.GetShape);
        set => CallVoid(MethodName.SetShape, (int)value);
    }

    public InterestOrientation Orientation
    {
        get => (InterestOrientation)CallInt(MethodName.GetOrientation);
        set => CallVoid(MethodName.SetOrientation, (int)value);
    }

    public int GridSize
    {
        get => CallInt(MethodName.GetGridSize);
        set => CallVoid(MethodName.SetGridSize, value);
    }

    public float Radius
    {
        get => CallFloat(MethodName.GetRadius);
        set => CallVoid(MethodName.SetRadius, value);
    }

    public float FovAngle
    {
        get => CallFloat(MethodName.GetFovAngle);
        set => CallVoid(MethodName.SetFovAngle, value);
    }

    public int BaseSendRate
    {
        get => CallInt(MethodName.GetBaseSendRate);
        set => CallVoid(MethodName.SetBaseSendRate, value);
    }

    public DecayMode DecayMode
    {
        get => (DecayMode)CallInt(MethodName.GetDecayMode);
        set => CallVoid(MethodName.SetDecayMode, (int)value);
    }

    public NodePath TargetPath
    {
        get => Call(MethodName.GetTargetPath).As<NodePath>();
        set => CallVoid(MethodName.SetTargetPath, value);
    }

    public bool Enabled
    {
        get => CallBool(MethodName.GetEnabled);
        set => CallVoid(MethodName.SetEnabled, value);
    }

    public bool DebugDraw
    {
        get => CallBool(MethodName.GetDebugDraw);
        set => CallVoid(MethodName.SetDebugDraw, value);
    }

    public DebugLabels DebugLabels
    {
        get => (DebugLabels)CallInt(MethodName.GetDebugLabels);
        set => CallVoid(MethodName.SetDebugLabels, (int)value);
    }

    public Gradient? DebugGradient
    {
        get => Call(MethodName.GetDebugGradient).As<Gradient>();
        set => CallVoid(MethodName.SetDebugGradient, value!);
    }

    // --- Cached native member names (GDExtension gets no GodotSharp-generated caches) ---

    /// <summary>Cached StringNames for this class's methods.</summary>
    public class MethodName : Node.MethodName
    {
        public static readonly StringName SetShape = "set_shape";
        public static readonly StringName GetShape = "get_shape";
        public static readonly StringName SetOrientation = "set_orientation";
        public static readonly StringName GetOrientation = "get_orientation";
        public static readonly StringName SetGridSize = "set_grid_size";
        public static readonly StringName GetGridSize = "get_grid_size";
        public static readonly StringName SetRadius = "set_radius";
        public static readonly StringName GetRadius = "get_radius";
        public static readonly StringName SetFovAngle = "set_fov_angle";
        public static readonly StringName GetFovAngle = "get_fov_angle";
        public static readonly StringName SetBaseSendRate = "set_base_send_rate";
        public static readonly StringName GetBaseSendRate = "get_base_send_rate";
        public static readonly StringName SetDecayMode = "set_decay_mode";
        public static readonly StringName GetDecayMode = "get_decay_mode";
        public static readonly StringName SetTargetPath = "set_target_path";
        public static readonly StringName GetTargetPath = "get_target_path";
        public static readonly StringName SetEnabled = "set_enabled";
        public static readonly StringName GetEnabled = "get_enabled";
        public static readonly StringName SetDebugDraw = "set_debug_draw";
        public static readonly StringName GetDebugDraw = "get_debug_draw";
        public static readonly StringName SetDebugLabels = "set_debug_labels";
        public static readonly StringName GetDebugLabels = "get_debug_labels";
        public static readonly StringName SetDebugGradient = "set_debug_gradient";
        public static readonly StringName GetDebugGradient = "get_debug_gradient";
    }

    /// <summary>Cached StringNames for this class's properties.</summary>
    public class PropertyName : Node.PropertyName
    {
        public static readonly StringName Shape = "shape";
        public static readonly StringName Orientation = "orientation";
        public static readonly StringName GridSize = "grid_size";
        public static readonly StringName Radius = "radius";
        public static readonly StringName FovAngle = "fov_angle";
        public static readonly StringName BaseSendRate = "base_send_rate";
        public static readonly StringName DecayMode = "decay_mode";
        public static readonly StringName TargetPath = "target_path";
        public static readonly StringName Enabled = "enabled";
        public static readonly StringName DebugDraw = "debug_draw";
        public static readonly StringName DebugLabels = "debug_labels";
        public static readonly StringName DebugGradient = "debug_gradient";
    }

    /// <summary>Cached StringNames for this class's signals.</summary>
    public class SignalName : Node.SignalName
    {
    }
}
