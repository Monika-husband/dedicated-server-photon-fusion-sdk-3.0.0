using Godot;

namespace FusionGodot;

/// <summary>
/// Accessors that turn an existing Fusion node into its typed wrapper without writing <c>new</c>.
/// Acquire a node with <see cref="Node.GetNode"/> and call one of these, e.g.
/// <c>var rep = GetNode("FusionReplicator").AsSharedReplicator();</c>.
/// Cache the result in a field rather than re-wrapping every frame.
/// </summary>
public static class NodeFusionExtensions
{
    // --- wrap an already-fetched node ---

    public static FusionReplicator AsReplicator(this Node node) => new(node);

    public static FusionSharedReplicator AsSharedReplicator(this Node node) => new(node);

    public static FusionServerReplicator AsServerReplicator(this Node node) => new(node);

    public static FusionSpawner AsSpawner(this Node node) => new(node);

    public static FusionInterestArea AsInterestArea(this Node node) => new(node);

    // --- fetch a child by path and wrap in one step ---

    public static FusionReplicator GetReplicator(this Node parent, NodePath path) => new(parent.GetNode(path));

    public static FusionSharedReplicator GetSharedReplicator(this Node parent, NodePath path) => new(parent.GetNode(path));

    public static FusionServerReplicator GetServerReplicator(this Node parent, NodePath path) => new(parent.GetNode(path));

    public static FusionSpawner GetSpawner(this Node parent, NodePath path) => new(parent.GetNode(path));

    public static FusionInterestArea GetInterestArea(this Node parent, NodePath path) => new(parent.GetNode(path));
}
