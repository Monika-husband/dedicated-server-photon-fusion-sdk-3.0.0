using System;
using Godot;
using Godot.Collections;

namespace FusionGodot;

#pragma warning disable CS8632

/// <summary>
/// Typed view over a native FusionSpawner node: spawns and despawns networked scenes.
/// </summary>
public sealed class FusionSpawner : FusionWrapper
{
    public FusionSpawner(GodotObject obj) : base(obj) { }

    /// <summary>The wrapped node.</summary>
    public Node Node => (Node)Obj;

    // --- Spawnable scene registry ---

    public void AddSpawnableScene(PackedScene scene) => CallVoid(MethodName.AddSpawnableScene, scene);

    public void RemoveSpawnableScene(PackedScene scene) => CallVoid(MethodName.RemoveSpawnableScene, scene);

    public int GetSpawnableSceneCount() => CallInt(MethodName.GetSpawnableSceneCount);

    public PackedScene GetSpawnableScene(int index) => Call(MethodName.GetSpawnableScene, index).As<PackedScene>();

    public void ClearSpawnableScenes() => CallVoid(MethodName.ClearSpawnableScenes);

    public Array<PackedScene> SpawnableScenesList
    {
        get => Call(MethodName.GetSpawnableScenesList).As<Array<PackedScene>>();
        set => CallVoid(MethodName.SetSpawnableScenesList, value);
    }

    // --- Spawn target / mode ---

    public NodePath SpawnPath
    {
        get => Call(MethodName.GetSpawnPath).As<NodePath>();
        set => CallVoid(MethodName.SetSpawnPath, value);
    }

    public bool SpawnAsSubObject
    {
        get => CallBool(MethodName.IsSpawnAsSubObject);
        set => CallVoid(MethodName.SetSpawnAsSubObject, value);
    }

    public bool IsSubSpawner() => CallBool(MethodName.IsSubSpawner);

    public SpawnMapMode SpawnMapMode
    {
        get => (SpawnMapMode)CallInt(MethodName.GetSpawnMapMode);
        set => CallVoid(MethodName.SetSpawnMapMode, (int)value);
    }

    public int BoundMapSequence
    {
        get => CallInt(MethodName.GetBoundMapSequence);
        set => CallVoid(MethodName.SetBoundMapSequence, value);
    }

    // --- Spawn / despawn ---

    /// <summary>
    /// Spawn a networked instance. Pass a scene (or null to use the spawner's default) and an
    /// optional pre-spawn Callable invoked on the new node before it goes live. Returns the node.
    /// </summary>
    public Node? Spawn(PackedScene? scene = null, Callable? preSpawnFunction = null)
        => Call(MethodName.Spawn, scene!, preSpawnFunction ?? new Callable()).As<Node>();

    public void Despawn(Node node) => CallVoid(MethodName.Despawn, node);

    public void BindRootReplicator(FusionReplicator replicator) => CallVoid(MethodName.BindRootReplicator, replicator.Self);

    // --- Signals ---

    /// <summary>Fires when a node is spawned through this spawner. Handler receives the new node.</summary>
    public event Action<Node> Spawned
    {
        add => Obj.Connect(SignalName.Spawned, Callable.From(value));
        remove => Obj.Disconnect(SignalName.Spawned, Callable.From(value));
    }

    /// <summary>Fires when a node is despawned through this spawner. Handler receives the removed node.</summary>
    public event Action<Node> Despawned
    {
        add => Obj.Connect(SignalName.Despawned, Callable.From(value));
        remove => Obj.Disconnect(SignalName.Despawned, Callable.From(value));
    }

    // --- Cached native member names (GDExtension gets no GodotSharp-generated caches) ---

    /// <summary>Cached StringNames for this class's methods.</summary>
    public class MethodName : Node.MethodName
    {
        public static readonly StringName AddSpawnableScene = "add_spawnable_scene";
        public static readonly StringName RemoveSpawnableScene = "remove_spawnable_scene";
        public static readonly StringName GetSpawnableSceneCount = "get_spawnable_scene_count";
        public static readonly StringName GetSpawnableScene = "get_spawnable_scene";
        public static readonly StringName ClearSpawnableScenes = "clear_spawnable_scenes";
        public static readonly StringName SetSpawnPath = "set_spawn_path";
        public static readonly StringName GetSpawnPath = "get_spawn_path";
        public static readonly StringName SetSpawnableScenesList = "set_spawnable_scenes_list";
        public static readonly StringName GetSpawnableScenesList = "get_spawnable_scenes_list";
        public static readonly StringName Spawn = "spawn";
        public static readonly StringName Despawn = "despawn";
        public static readonly StringName SetSpawnAsSubObject = "set_spawn_as_sub_object";
        public static readonly StringName IsSpawnAsSubObject = "is_spawn_as_sub_object";
        public static readonly StringName BindRootReplicator = "bind_root_replicator";
        public static readonly StringName IsSubSpawner = "is_sub_spawner";
        public static readonly StringName SetSpawnMapMode = "set_spawn_map_mode";
        public static readonly StringName GetSpawnMapMode = "get_spawn_map_mode";
        public static readonly StringName SetBoundMapSequence = "set_bound_map_sequence";
        public static readonly StringName GetBoundMapSequence = "get_bound_map_sequence";
    }

    /// <summary>Cached StringNames for this class's properties.</summary>
    public class PropertyName : Node.PropertyName
    {
        public static readonly StringName SpawnPath = "spawn_path";
        public static readonly StringName SpawnAsSubObject = "spawn_as_sub_object";
        public static readonly StringName SpawnMapMode = "spawn_map_mode";
        public static readonly StringName SpawnableScenes = "spawnable_scenes";
    }

    /// <summary>Cached StringNames for this class's signals.</summary>
    public class SignalName : Node.SignalName
    {
        public static readonly StringName Spawned = "spawned";
        public static readonly StringName Despawned = "despawned";
    }
}
