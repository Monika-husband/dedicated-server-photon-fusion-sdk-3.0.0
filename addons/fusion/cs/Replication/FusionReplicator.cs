using System;
using Godot;

namespace FusionGodot;

#pragma warning disable CS8632

/// <summary>
/// Typed view over a native FusionReplicator node (the component that replicates a
/// networked object's state). The native class is abstract; concrete nodes are a
/// <see cref="FusionSharedReplicator"/> or <see cref="FusionServerReplicator"/>, but this
/// base wrapper exposes the shared surface and can wrap either.
/// </summary>
public class FusionReplicator : FusionWrapper
{
    public FusionReplicator(GodotObject obj) : base(obj) { }

    /// <summary>The wrapped node.</summary>
    public Node Node => (Node)Obj;

    // --- Authority / ownership ---

    /// <summary>True when this client owns the object and may modify its replicated state.</summary>
    public bool HasAuthority() => CallBool(MethodName.HasAuthority);

    /// <summary>Player id of the current owner.</summary>
    public int GetOwnerId() => CallInt(MethodName.GetOwnerId);

    public OwnerMode OwnerMode
    {
        get => (OwnerMode)CallInt(MethodName.GetOwnerMode);
        set => CallVoid(MethodName.SetOwnerMode, (int)value);
    }

    // --- Core config ---

    public NodePath RootPath
    {
        get => Call(MethodName.GetRootPath).As<NodePath>();
        set => CallVoid(MethodName.SetRootPath, value);
    }

    /// <summary>The replication config resource (native FusionReplicationConfig). Exposed as a Resource.</summary>
    public Resource? ReplicationConfig
    {
        get => Call(MethodName.GetReplicationConfig).As<Resource>();
        set => CallVoid(MethodName.SetReplicationConfig, value!);
    }

    public int UpdateInterval
    {
        get => CallInt(MethodName.GetUpdateInterval);
        set => CallVoid(MethodName.SetUpdateInterval, value);
    }

    public float ObjectInterpolationTime
    {
        get => CallFloat(MethodName.GetObjectInterpolationTime);
        set => CallVoid(MethodName.SetObjectInterpolationTime, value);
    }

    // --- Interest / area of interest ---

    public InterestMode InterestMode
    {
        get => (InterestMode)CallInt(MethodName.GetInterestMode);
        set => CallVoid(MethodName.SetInterestMode, (int)value);
    }

    public int InterestKey
    {
        get => CallInt(MethodName.GetInterestKey);
        set => CallVoid(MethodName.SetInterestKey, value);
    }

    public bool InterestDestroyOnExit
    {
        get => CallBool(MethodName.GetInterestDestroyOnExit);
        set => CallVoid(MethodName.SetInterestDestroyOnExit, value);
    }

    // --- Root smoothing / physics correction (inspector-configurable; settable at runtime) ---

    public ReplicationMode RootReplicationMode
    {
        get => (ReplicationMode)CallInt(MethodName.GetRootReplicationMode);
        set => CallVoid(MethodName.SetRootReplicationMode, (int)value);
    }

    public bool RootGlobalCoordinates
    {
        get => CallBool(MethodName.GetRootGlobalCoordinates);
        set => CallVoid(MethodName.SetRootGlobalCoordinates, value);
    }

    public InterpolationMode RootInterpolationMode
    {
        get => (InterpolationMode)CallInt(MethodName.GetRootInterpolationMode);
        set => CallVoid(MethodName.SetRootInterpolationMode, (int)value);
    }

    public float RootTeleportThreshold
    {
        get => CallFloat(MethodName.GetRootTeleportThreshold);
        set => CallVoid(MethodName.SetRootTeleportThreshold, value);
    }

    public float RootRotationTeleportThreshold
    {
        get => CallFloat(MethodName.GetRootRotationTeleportThreshold);
        set => CallVoid(MethodName.SetRootRotationTeleportThreshold, value);
    }

    public TeleportSnapMode RootTeleportSnapMode
    {
        get => (TeleportSnapMode)CallInt(MethodName.GetRootTeleportSnapMode);
        set => CallVoid(MethodName.SetRootTeleportSnapMode, (int)value);
    }

    public float RootSpringStiffness
    {
        get => CallFloat(MethodName.GetRootSpringStiffness);
        set => CallVoid(MethodName.SetRootSpringStiffness, value);
    }

    public float RootSpringDamping
    {
        get => CallFloat(MethodName.GetRootSpringDamping);
        set => CallVoid(MethodName.SetRootSpringDamping, value);
    }

    public float RootSnapDistance
    {
        get => CallFloat(MethodName.GetRootSnapDistance);
        set => CallVoid(MethodName.SetRootSnapDistance, value);
    }

    public PhysicsCorrectionMode RootCorrectionMode
    {
        get => (PhysicsCorrectionMode)CallInt(MethodName.GetRootCorrectionMode);
        set => CallVoid(MethodName.SetRootCorrectionMode, (int)value);
    }

    public float RootBlendFactor
    {
        get => CallFloat(MethodName.GetRootBlendFactor);
        set => CallVoid(MethodName.SetRootBlendFactor, value);
    }

    public float RootMaxForecastTime
    {
        get => CallFloat(MethodName.GetRootMaxForecastTime);
        set => CallVoid(MethodName.SetRootMaxForecastTime, value);
    }

    public bool RootForecastGravity
    {
        get => CallBool(MethodName.GetRootForecastGravity);
        set => CallVoid(MethodName.SetRootForecastGravity, value);
    }

    public float RootMinPositionError
    {
        get => CallFloat(MethodName.GetRootMinPositionError);
        set => CallVoid(MethodName.SetRootMinPositionError, value);
    }

    public float RootMinRotationError
    {
        get => CallFloat(MethodName.GetRootMinRotationError);
        set => CallVoid(MethodName.SetRootMinRotationError, value);
    }

    public float RootJumpLandSpeedup
    {
        get => CallFloat(MethodName.GetRootJumpLandSpeedup);
        set => CallVoid(MethodName.SetRootJumpLandSpeedup, value);
    }

    public float RootJumpLandDecayTime
    {
        get => CallFloat(MethodName.GetRootJumpLandDecayTime);
        set => CallVoid(MethodName.SetRootJumpLandDecayTime, value);
    }

    public float RootCollisionCooldown
    {
        get => CallFloat(MethodName.GetRootCollisionCooldown);
        set => CallVoid(MethodName.SetRootCollisionCooldown, value);
    }

    public float RootCollisionRampTime
    {
        get => CallFloat(MethodName.GetRootCollisionRampTime);
        set => CallVoid(MethodName.SetRootCollisionRampTime, value);
    }

    public bool SyncSleeping
    {
        get => CallBool(MethodName.GetSyncSleeping);
        set => CallVoid(MethodName.SetSyncSleeping, value);
    }

    public float MaxSleepIgnoreTime
    {
        get => CallFloat(MethodName.GetMaxSleepIgnoreTime);
        set => CallVoid(MethodName.SetMaxSleepIgnoreTime, value);
    }

    // --- Actions ---

    /// <summary>Emit a teleport snap on the next sync (non-physics objects).</summary>
    public void Teleport() => CallVoid(MethodName.Teleport);

    public void Teleport2D(Vector2 position, float rotation) => CallVoid(MethodName.Teleport2D, position, rotation);

    public void Teleport3D(Vector3 position, Vector3 rotation) => CallVoid(MethodName.Teleport3D, position, rotation);

    /// <summary>Force-copy the Fusion buffer to the node, bypassing smoothing.</summary>
    public void ResetState() => CallVoid(MethodName.ResetState);

    public void ResetForecast() => CallVoid(MethodName.ResetForecast);

    public int ComputeWordCount() => CallInt(MethodName.ComputeWordCount);

    public float GetSpawnTimestamp() => CallFloat(MethodName.GetSpawnTimestamp);

    /// <summary>Override the interpolation strategy for a single replicated property at runtime.</summary>
    public void SetPropertyInterpolation(NodePath property, Interpolate mode, Callable callable = default)
        => CallVoid(MethodName.SetPropertyInterpolation, property, (int)mode, callable);

    /// <summary>Read the latest authoritative ("shadow") value for a replicated property (physics replication).</summary>
    public Variant GetShadowValue(NodePath path) => Call(MethodName.GetShadowValue, path);

    public bool HasShadowData() => CallBool(MethodName.HasShadowData);

    // --- Signals ---

    /// <summary>Fires when ownership of this object changes. Handler receives the new authority flag.</summary>
    public event Action<bool> AuthorityChanged
    {
        add => Obj.Connect(SignalName.AuthorityChanged, Callable.From(value));
        remove => Obj.Disconnect(SignalName.AuthorityChanged, Callable.From(value));
    }

    /// <summary>Fires when the object's state is reset (e.g. on resimulation). Handler receives the reset info.</summary>
    public event Action<FusionStateResetInfo> StateReset
    {
        add => ConvertedEvent<GodotObject, FusionStateResetInfo>(
                      SignalName.StateReset, info => new FusionStateResetInfo(info), value, true);
        remove => ConvertedEvent<GodotObject, FusionStateResetInfo>(
                      SignalName.StateReset, info => new FusionStateResetInfo(info), value, false);
    }

    /// <summary>Fires once the object has spawned and is network-ready.</summary>
    public event Action Spawned
    {
        add => Obj.Connect(SignalName.Spawned, Callable.From(value));
        remove => Obj.Disconnect(SignalName.Spawned, Callable.From(value));
    }

    // --- Cached native member names (GDExtension gets no GodotSharp-generated caches) ---

    /// <summary>Cached StringNames for this class's methods.</summary>
    public class MethodName : Node.MethodName
    {
        public static readonly StringName SetRootPath = "set_root_path";
        public static readonly StringName GetRootPath = "get_root_path";
        public static readonly StringName SetReplicationConfig = "set_replication_config";
        public static readonly StringName GetReplicationConfig = "get_replication_config";
        public static readonly StringName SetUpdateInterval = "set_update_interval";
        public static readonly StringName GetUpdateInterval = "get_update_interval";
        public static readonly StringName SetObjectInterpolationTime = "set_object_interpolation_time";
        public static readonly StringName GetObjectInterpolationTime = "get_object_interpolation_time";
        public static readonly StringName SetOwnerMode = "set_owner_mode";
        public static readonly StringName GetOwnerMode = "get_owner_mode";
        public static readonly StringName HasAuthority = "has_authority";
        public static readonly StringName GetOwnerId = "get_owner_id";
        public static readonly StringName SetInterestMode = "set_interest_mode";
        public static readonly StringName GetInterestMode = "get_interest_mode";
        public static readonly StringName SetInterestKey = "set_interest_key";
        public static readonly StringName GetInterestKey = "get_interest_key";
        public static readonly StringName SetInterestDestroyOnExit = "set_interest_destroy_on_exit";
        public static readonly StringName GetInterestDestroyOnExit = "get_interest_destroy_on_exit";
        public static readonly StringName SetRootReplicationMode = "set_root_replication_mode";
        public static readonly StringName GetRootReplicationMode = "get_root_replication_mode";
        public static readonly StringName SetRootTeleportThreshold = "set_root_teleport_threshold";
        public static readonly StringName GetRootTeleportThreshold = "get_root_teleport_threshold";
        public static readonly StringName SetRootRotationTeleportThreshold = "set_root_rotation_teleport_threshold";
        public static readonly StringName GetRootRotationTeleportThreshold = "get_root_rotation_teleport_threshold";
        public static readonly StringName SetRootTeleportSnapMode = "set_root_teleport_snap_mode";
        public static readonly StringName GetRootTeleportSnapMode = "get_root_teleport_snap_mode";
        public static readonly StringName SetRootSpringStiffness = "set_root_spring_stiffness";
        public static readonly StringName GetRootSpringStiffness = "get_root_spring_stiffness";
        public static readonly StringName SetRootSpringDamping = "set_root_spring_damping";
        public static readonly StringName GetRootSpringDamping = "get_root_spring_damping";
        public static readonly StringName SetRootInterpolationMode = "set_root_interpolation_mode";
        public static readonly StringName GetRootInterpolationMode = "get_root_interpolation_mode";
        public static readonly StringName SetRootSnapDistance = "set_root_snap_distance";
        public static readonly StringName GetRootSnapDistance = "get_root_snap_distance";
        public static readonly StringName SetRootJumpLandSpeedup = "set_root_jump_land_speedup";
        public static readonly StringName GetRootJumpLandSpeedup = "get_root_jump_land_speedup";
        public static readonly StringName SetRootJumpLandDecayTime = "set_root_jump_land_decay_time";
        public static readonly StringName GetRootJumpLandDecayTime = "get_root_jump_land_decay_time";
        public static readonly StringName SetRootMaxForecastTime = "set_root_max_forecast_time";
        public static readonly StringName GetRootMaxForecastTime = "get_root_max_forecast_time";
        public static readonly StringName SetRootForecastGravity = "set_root_forecast_gravity";
        public static readonly StringName GetRootForecastGravity = "get_root_forecast_gravity";
        public static readonly StringName SetRootGlobalCoordinates = "set_root_global_coordinates";
        public static readonly StringName GetRootGlobalCoordinates = "get_root_global_coordinates";
        public static readonly StringName SetRootCorrectionMode = "set_root_correction_mode";
        public static readonly StringName GetRootCorrectionMode = "get_root_correction_mode";
        public static readonly StringName SetRootBlendFactor = "set_root_blend_factor";
        public static readonly StringName GetRootBlendFactor = "get_root_blend_factor";
        public static readonly StringName SetRootMinPositionError = "set_root_min_position_error";
        public static readonly StringName GetRootMinPositionError = "get_root_min_position_error";
        public static readonly StringName SetRootMinRotationError = "set_root_min_rotation_error";
        public static readonly StringName GetRootMinRotationError = "get_root_min_rotation_error";
        public static readonly StringName SetSyncSleeping = "set_sync_sleeping";
        public static readonly StringName GetSyncSleeping = "get_sync_sleeping";
        public static readonly StringName SetMaxSleepIgnoreTime = "set_max_sleep_ignore_time";
        public static readonly StringName GetMaxSleepIgnoreTime = "get_max_sleep_ignore_time";
        public static readonly StringName SetRootCollisionCooldown = "set_root_collision_cooldown";
        public static readonly StringName GetRootCollisionCooldown = "get_root_collision_cooldown";
        public static readonly StringName SetRootCollisionRampTime = "set_root_collision_ramp_time";
        public static readonly StringName GetRootCollisionRampTime = "get_root_collision_ramp_time";
        public static readonly StringName OnBodyCollision = "_on_body_collision";
        public static readonly StringName OnReplicationConfigChanged = "_on_replication_config_changed";
        public static readonly StringName OnSceneTreeChanged = "_on_scene_tree_changed";
        public static readonly StringName ResetForecast = "reset_forecast";
        public static readonly StringName GetShadowValue = "get_shadow_value";
        public static readonly StringName HasShadowData = "has_shadow_data";
        public static readonly StringName GetSpawnTimestamp = "get_spawn_timestamp";
        public static readonly StringName Teleport = "teleport";
        public static readonly StringName Teleport2D = "teleport_2d";
        public static readonly StringName Teleport3D = "teleport_3d";
        public static readonly StringName SetPropertyInterpolation = "set_property_interpolation";
        public static readonly StringName ComputeWordCount = "compute_word_count";
        public static readonly StringName ResetState = "reset_state";
    }

    /// <summary>Cached StringNames for this class's properties.</summary>
    public class PropertyName : Node.PropertyName
    {
        public static readonly StringName RootPath = "root_path";
        public static readonly StringName ReplicationConfig = "replication_config";
        public static readonly StringName UpdateInterval = "update_interval";
        public static readonly StringName ObjectInterpolationTime = "object_interpolation_time";
        public static readonly StringName InterestMode = "interest_mode";
        public static readonly StringName InterestKey = "interest_key";
        public static readonly StringName InterestDestroyOnExit = "interest_destroy_on_exit";
        public static readonly StringName RootReplicationMode = "root_replication_mode";
        public static readonly StringName RootGlobalCoordinates = "root_global_coordinates";
        public static readonly StringName RootInterpolationMode = "root_interpolation_mode";
        public static readonly StringName RootTeleportThreshold = "root_teleport_threshold";
        public static readonly StringName RootRotationTeleportThreshold = "root_rotation_teleport_threshold";
        public static readonly StringName RootTeleportSnapMode = "root_teleport_snap_mode";
        public static readonly StringName RootSpringStiffness = "root_spring_stiffness";
        public static readonly StringName RootSpringDamping = "root_spring_damping";
        public static readonly StringName RootSnapDistance = "root_snap_distance";
        public static readonly StringName RootJumpLandSpeedup = "root_jump_land_speedup";
        public static readonly StringName RootJumpLandDecayTime = "root_jump_land_decay_time";
        public static readonly StringName RootMaxForecastTime = "root_max_forecast_time";
        public static readonly StringName RootForecastGravity = "root_forecast_gravity";
        public static readonly StringName RootCorrectionMode = "root_correction_mode";
        public static readonly StringName RootBlendFactor = "root_blend_factor";
        public static readonly StringName RootCollisionCooldown = "root_collision_cooldown";
        public static readonly StringName RootCollisionRampTime = "root_collision_ramp_time";
        public static readonly StringName RootMinPositionError = "root_min_position_error";
        public static readonly StringName RootMinRotationError = "root_min_rotation_error";
        public static readonly StringName SyncSleeping = "sync_sleeping";
        public static readonly StringName MaxSleepIgnoreTime = "max_sleep_ignore_time";
    }

    /// <summary>Cached StringNames for this class's signals.</summary>
    public class SignalName : Node.SignalName
    {
        public static readonly StringName AuthorityChanged = "authority_changed";
        public static readonly StringName StateReset = "state_reset";
        public static readonly StringName Spawned = "spawned";
    }
}
