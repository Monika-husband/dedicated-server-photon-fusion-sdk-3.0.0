namespace FusionGodot;

// Enum values mirror the native GDExtension constants exactly (see
// godot-fusion/src/fusion_client.h, fusion_replicator.h, fusion_spawner.h).
// They are passed to/from the engine as ints, so the underlying values MUST
// match the C++ side.

/// <summary>Connection lifecycle state. Mirrors FusionClient::ConnectionStatus.</summary>
public enum ConnectionStatus
{
    Disconnected = 0,
    ConnectingToPhoton = 1,
    ConnectedToPhoton = 2,
    JoiningRoom = 3,
    InRoom = 4,
    Error = 5,
}

/// <summary>Built-in RPC target groups. Mirrors FusionClient::RpcTarget.</summary>
/// <remarks>Positive integers address a specific player id instead of a group.</remarks>
public enum RpcTarget
{
    All = 0,
    Master = -1,
    Plugin = -2,
    Owner = -3,
}

/// <summary>RPC failure reason. Mirrors FusionClient::RpcError.</summary>
public enum RpcError
{
    PlayerMissing = 32,
    ObjectMissing = 64,
    MapIncorrect = 128,
    MaxAttempts = 256,
}

/// <summary>Scene load strategy. Mirrors FusionClient::SceneLoadMode.</summary>
public enum SceneLoadMode
{
    Auto = 0,
    Custom = 1,
}

/// <summary>Network topology simulation. Mirrors FusionClient::SimulationMode.</summary>
public enum SimulationMode
{
    Shared = 0,
    ClientServer = 1,
}

/// <summary>Root node replication mode. Mirrors FusionReplicator::ReplicationMode.</summary>
public enum ReplicationMode
{
    None = 0,
    Auto = 1,
}

/// <summary>Physics correction strategy. Mirrors FusionReplicator::PhysicsCorrectionMode.</summary>
public enum PhysicsCorrectionMode
{
    Velocity = 0,
    Force = 1,
    Blend = 2,
}

/// <summary>Teleport snapping behaviour. Mirrors FusionReplicator::TeleportSnapMode.</summary>
public enum TeleportSnapMode
{
    Source = 0,
    Projected = 1,
}

/// <summary>Ownership model. Mirrors FusionReplicator::OwnerMode (note: 4 is intentionally absent).</summary>
public enum OwnerMode
{
    Transaction = 0,
    PlayerAttached = 1,
    Dynamic = 2,
    MasterClient = 3,
    PlayerPredicted = 5,
}

/// <summary>Area-of-interest mode. Mirrors FusionReplicator::InterestMode.</summary>
public enum InterestMode
{
    Global = 0,
    Area = 1,
    User = 2,
}

/// <summary>Root interpolation strategy. Mirrors FusionReplicator::InterpolationMode.</summary>
public enum InterpolationMode
{
    None = 0,
    Exponential = 1,
    Forecast = 2,
}

/// <summary>Spawner-to-map binding. Mirrors FusionSpawner::SpawnMapMode.</summary>
public enum SpawnMapMode
{
    Unbound = 0,
    Bound = 1,
}

/// <summary>Per-property interpolation strategy. Mirrors FusionReplicationConfig::Interpolate.</summary>
public enum Interpolate
{
    None = 0,
    Lerp = 1,
    Angle = 2,
}

/// <summary>Why a replicator's state was reset. Mirrors FusionStateResetInfo::Reason.</summary>
public enum Reason
{
    RemoteReceived = 0,
    PredictionReset = 1,
    StateOverride = 2,
}

/// <summary>Interest area shape. Mirrors FusionInterestArea::Shape.</summary>
public enum InterestShape
{
    Square = 0,
    Circle = 1,
    Cone = 2,
}

/// <summary>Interest area plane/axis orientation. Mirrors FusionInterestArea::Orientation.</summary>
public enum InterestOrientation
{
    TwoD = 0,
    ThreeDXZ = 1,
    ThreeDXY = 2,
}

/// <summary>Interest priority decay across rings. Mirrors FusionInterestArea::DecayMode.</summary>
public enum DecayMode
{
    Flat = 0,
    Doubling = 1,
    Linear = 2,
}

/// <summary>Interest area debug label mode. Mirrors FusionInterestArea::DebugLabels.</summary>
public enum DebugLabels
{
    None = 0,
    Priority = 1,
    CellAndPriority = 2,
}
