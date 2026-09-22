using System;
using System.Collections.Generic;
using Godot;
using Array = Godot.Collections.Array;

namespace FusionGodot;

#pragma warning disable CS8632

/// <summary>
/// Typed facade over the global <c>Fusion</c> engine singleton (native FusionClient).
/// Mirrors GDScript usage such as <c>Fusion.rpc(callable, arg)</c> as <c>Fusion.Rpc(callable, arg)</c>.
/// </summary>
/// <remarks>
/// All calls forward through <see cref="GodotObject.Call(StringName, Variant[])"/> on the
/// cached singleton instance, so this is pure public-API and needs no GodotSharp internals.
/// </remarks>
public static class Fusion
{
    private static GodotObject? _singleton;

    /// <summary>The cached native FusionClient singleton. Throws if Fusion is not registered.</summary>
    public static GodotObject Singleton => _singleton ??= Engine.GetSingleton("Fusion")
        ?? throw new InvalidOperationException(
            "The 'Fusion' singleton is not registered. Is the GodotFusion GDExtension enabled?");

    // --- Initialization & config ---

    public static bool IsInitialized() => Singleton.Call(MethodName.IsInitialized).AsBool();

    public static void SetAppId(string appId) => Singleton.Call(MethodName.SetAppId, appId);

    public static string GetDefaultRegion() => Singleton.Call(MethodName.GetDefaultRegion).AsString();

    public static string GetConfiguredAppId() => Singleton.Call(MethodName.GetConfiguredAppId).AsString();

    public static string GetConfiguredAppVersion() => Singleton.Call(MethodName.GetConfiguredAppVersion).AsString();

    // --- Connection & rooms ---

    public static void ConnectToPhoton(string userId = "", string region = "", string appVersion = "")
        => Singleton.Call(MethodName.ConnectToPhoton, userId, region, appVersion);

    public static void DisconnectFromPhoton() => Singleton.Call(MethodName.DisconnectFromPhoton);

    public static void JoinRoom(string name = "", FusionRoomOptions? options = null)
        => Singleton.Call(MethodName.JoinRoom, name, options?.Obj!);

    public static void JoinOrCreateRoom(string name = "", FusionRoomOptions? options = null)
        => Singleton.Call(MethodName.JoinOrCreateRoom, name, options?.Obj!);

    public static void CreateRoom(string name = "", FusionRoomOptions? options = null)
        => Singleton.Call(MethodName.CreateRoom, name, options?.Obj!);

    public static void LeaveRoom() => Singleton.Call(MethodName.LeaveRoom);

    public static FusionRoom? GetRoom()
        => Singleton.Call(MethodName.GetRoom).AsWrapper(o => new FusionRoom(o));

    public static List<FusionRoomListing> GetRoomList()
        => Singleton.Call(MethodName.GetRoomList).ToWrappers(o => new FusionRoomListing(o));

    // --- Status & info ---

    public static ConnectionStatus GetConnectionStatus()
        => (ConnectionStatus)Singleton.Call(MethodName.GetConnectionStatus).AsInt32();

    public static bool IsConnectedToPhoton() => Singleton.Call(MethodName.IsConnectedToPhoton).AsBool();

    public static bool IsInRoom() => Singleton.Call(MethodName.IsInRoom).AsBool();

    public static bool IsMasterClient() => Singleton.Call(MethodName.IsMasterClient).AsBool();

    public static int GetLocalPlayerId() => Singleton.Call(MethodName.GetLocalPlayerId).AsInt32();

    public static SimulationMode GetSimulationMode()
        => (SimulationMode)Singleton.Call(MethodName.GetSimulationMode).AsInt32();

    public static double GetNetworkTime() => Singleton.Call(MethodName.GetNetworkTime).AsDouble();

    public static double GetRtt() => Singleton.Call(MethodName.GetRtt).AsDouble();

    // --- Scene management ---

    public static void SetSceneLoadMode(SceneLoadMode mode) => Singleton.Call(MethodName.SetSceneLoadMode, (int)mode);

    public static SceneLoadMode GetSceneLoadMode()
        => (SceneLoadMode)Singleton.Call(MethodName.GetSceneLoadMode).AsInt32();

    public static void SetSceneParent(Node parent) => Singleton.Call(MethodName.SetSceneParent, parent);

    /// <summary>Install a custom scene loader (used when scene load mode is Custom). The Callable performs the load.</summary>
    public static void SetSceneLoader(Callable loader) => Singleton.Call(MethodName.SetSceneLoader, loader);

    public static Node GetSceneParent() => Singleton.Call(MethodName.GetSceneParent).As<Node>();

    public static void LoadScene(PackedScene scene) => Singleton.Call(MethodName.LoadScene, scene);

    public static void RegisterCurrentScene() => Singleton.Call(MethodName.RegisterCurrentScene);

    public static void UnloadScene(int sequence) => Singleton.Call(MethodName.UnloadScene, sequence);

    public static void DestroySceneObject(Node node) => Singleton.Call(MethodName.DestroySceneObject, node);

    public static void NotifySceneReady(Node sceneRoot, int sequence)
        => Singleton.Call(MethodName.NotifySceneReady, sceneRoot, sequence);

    public static Array GetLoadedScenes() => Singleton.Call(MethodName.GetLoadedScenes).AsGodotArray();

    // --- RPCs ---
    // RPC TO ALL
    /// <summary>Invoke an RPC by delegate on all peers (per the method's @rpc config).</summary>
    public static FusionRpcResult Rpc(Delegate method, params Variant[] args)
        => new(Singleton.Call(MethodName.Rpc, Prepend(ToCallable(method), args)).AsGodotObject());

    /// <summary>Invoke an RPC by name on all peers (per the method's @rpc config).</summary>
    public static FusionRpcResult Rpc(GodotObject targetNode, StringName method, params Variant[] args)
        => new(Singleton.Call(MethodName.Rpc, Prepend(new Callable(targetNode, method), args)).AsGodotObject());


    // RPC TO TARGET ENUM
    /// <summary>Invoke an RPC by delegate targeting a built-in group or a specific player id.</summary>
    public static FusionRpcResult RpcTo(RpcTarget target, Delegate method, params Variant[] args)
        => RpcTo((int)target, method, args);

    /// <summary>Invoke an RPC by name targeting a built-in group or a specific player id.</summary>
    public static FusionRpcResult RpcTo(RpcTarget target, GodotObject targetNode, StringName method, params Variant[] args)
        => RpcTo((int)target, targetNode, method, args);


    // RPC TO TARGET RAW INTEGER
    /// <summary>Invoke an RPC by delegate targeting a raw target value (group enum value or positive player id).</summary>
    public static FusionRpcResult RpcTo(int target, Delegate method, params Variant[] args)
        => new(Singleton.Call(MethodName.RpcTo, Prepend(target, ToCallable(method), args)).AsGodotObject());

    /// <summary>Invoke an RPC by name targeting a raw target value (group enum value or positive player id).</summary>
    public static FusionRpcResult RpcTo(int target, GodotObject targetNode, StringName method, params Variant[] args)
        => new(Singleton.Call(MethodName.RpcTo, Prepend(target, new Callable(targetNode, method), args)).AsGodotObject());

    // RPC TO PLAYER
    /// <summary>Invoke an RPC by delegate targeting a player id (per the method's @rpc config).</summary>
    public static FusionRpcResult RpcToPlayer(int playerId, Delegate method, params Variant[] args)
        => new(Singleton.Call(MethodName.RpcToPlayer, Prepend(playerId, ToCallable(method), args)).AsGodotObject());

    /// <summary>Invoke an RPC by name targeting a player id.</summary>
    public static FusionRpcResult RpcToPlayer(int playerId, GodotObject targetNode, StringName method, params Variant[] args)
        => new(Singleton.Call(MethodName.RpcToPlayer, Prepend(playerId, new Callable(targetNode, method), args)).AsGodotObject());

    /// <summary>Context of the RPC currently being handled (valid only inside an RPC method).</summary>
    public static FusionRpcInfo? GetRpcInfo()
        => Singleton.Call(MethodName.GetRpcInfo).AsWrapper(o => new FusionRpcInfo(o));

    public static int GetRpcSender() => Singleton.Call(MethodName.GetRpcSender).AsInt32();

    public static void RegisterBroadcastReceiver(Node node) => Singleton.Call(MethodName.RegisterBroadcastReceiver, node);

    public static void UnregisterBroadcastReceiver(Node node) => Singleton.Call(MethodName.UnregisterBroadcastReceiver, node);

    // --- Interest / area of interest ---

    public static void SetAreaKeys(Array keys, int playerId = -1) => Singleton.Call(MethodName.SetAreaKeys, keys, playerId);

    public static Array GetAreaKeys(int playerId = -1) => Singleton.Call(MethodName.GetAreaKeys, playerId).AsGodotArray();

    public static void ClearAreaKeys(int playerId = -1) => Singleton.Call(MethodName.ClearAreaKeys, playerId);

    public static void AddUserKey(int key, int sendRate = 0, int playerId = -1)
        => Singleton.Call(MethodName.AddUserKey, key, sendRate, playerId);

    public static void RemoveUserKey(int key, int playerId = -1) => Singleton.Call(MethodName.RemoveUserKey, key, playerId);

    public static void ClearUserKeys(int playerId = -1) => Singleton.Call(MethodName.ClearUserKeys, playerId);

    public static Array GetUserKeys(int playerId = -1) => Singleton.Call(MethodName.GetUserKeys, playerId).AsGodotArray();

    public static void ClearAllInterestKeys(int playerId = -1) => Singleton.Call(MethodName.ClearAllInterestKeys, playerId);

    // --- Signals (as C# events) ---

    public static event Action RoomJoined
    {
        add => Singleton.Connect(SignalName.RoomJoined, Callable.From(value));
        remove => Singleton.Disconnect(SignalName.RoomJoined, Callable.From(value));
    }

    public static event Action RoomLeft
    {
        add => Singleton.Connect(SignalName.RoomLeft, Callable.From(value));
        remove => Singleton.Disconnect(SignalName.RoomLeft, Callable.From(value));
    }

    public static event Action<Array> RoomListUpdated
    {
        add => Singleton.Connect(SignalName.RoomListUpdated, Callable.From(value));
        remove => Singleton.Disconnect(SignalName.RoomListUpdated, Callable.From(value));
    }

    public static event Action ConnectedToPhoton
    {
        add => Singleton.Connect(SignalName.ConnectedToPhoton, Callable.From(value));
        remove => Singleton.Disconnect(SignalName.ConnectedToPhoton, Callable.From(value));
    }

    public static event Action<string> ConnectionFailed
    {
        add => Singleton.Connect(SignalName.ConnectionFailed, Callable.From(value));
        remove => Singleton.Disconnect(SignalName.ConnectionFailed, Callable.From(value));
    }

    public static event Action<ConnectionStatus> ConnectionStatusChanged
    {
        add => Singleton.Connect(SignalName.ConnectionStatusChanged, Callable.From(value));
        remove => Singleton.Disconnect(SignalName.ConnectionStatusChanged, Callable.From(value));
    }

    public static event Action<int, PackedScene> SceneLoadRequested
    {
        add => Singleton.Connect(SignalName.SceneLoadRequested, Callable.From(value));
        remove => Singleton.Disconnect(SignalName.SceneLoadRequested, Callable.From(value));
    }

    public static event Action<int> SceneReady
    {
        add => Singleton.Connect(SignalName.SceneReady, Callable.From(value));
        remove => Singleton.Disconnect(SignalName.SceneReady, Callable.From(value));
    }

    public static event Action<int, string> SceneUnloaded
    {
        add => Singleton.Connect(SignalName.SceneUnloaded, Callable.From(value));
        remove => Singleton.Disconnect(SignalName.SceneUnloaded, Callable.From(value));
    }

    public static event Action<int, string> PlayerJoined
    {
        add => Singleton.Connect(SignalName.PlayerJoined, Callable.From(value));
        remove => Singleton.Disconnect(SignalName.PlayerJoined, Callable.From(value));
    }

    public static event Action<int, bool> PlayerLeft
    {
        add => Singleton.Connect(SignalName.PlayerLeft, Callable.From(value));
        remove => Singleton.Disconnect(SignalName.PlayerLeft, Callable.From(value));
    }

    public static event Action<int, int> MasterClientChanged
    {
        add => Singleton.Connect(SignalName.MasterClientChanged, Callable.From(value));
        remove => Singleton.Disconnect(SignalName.MasterClientChanged, Callable.From(value));
    }

    public static event Action<Node> InterestEnter
    {
        add => Singleton.Connect(SignalName.InterestEnter, Callable.From(value));
        remove => Singleton.Disconnect(SignalName.InterestEnter, Callable.From(value));
    }

    public static event Action<Node> InterestExit
    {
        add => Singleton.Connect(SignalName.InterestExit, Callable.From(value));
        remove => Singleton.Disconnect(SignalName.InterestExit, Callable.From(value));
    }

    // --- internals ---

    private static Variant[] Prepend(Variant first, Variant[] rest)
    {
        var all = new Variant[rest.Length + 1];
        all[0] = first;
        rest.CopyTo(all, 1);
        return all;
    }

    private static Variant[] Prepend(Variant first, Variant second, Variant[] rest)
    {
        var all = new Variant[rest.Length + 2];
        all[0] = first;
        all[1] = second;
        rest.CopyTo(all, 2);
        return all;
    }

    private static Callable ToCallable(Delegate method)
        => new Callable(method.Target as GodotObject, method.Method.Name);

    // --- Cached native member names (GDExtension gets no GodotSharp-generated caches) ---

    /// <summary>Cached StringNames for this class's methods.</summary>
    public class MethodName : GodotObject.MethodName
    {
        public static readonly StringName SetAppId = "set_app_id";
        public static readonly StringName IsInitialized = "is_initialized";
        public static readonly StringName GetDefaultRegion = "get_default_region";
        public static readonly StringName GetConfiguredAppId = "get_configured_app_id";
        public static readonly StringName GetConfiguredAppVersion = "get_configured_app_version";
        public static readonly StringName ConnectToPhoton = "connect_to_photon";
        public static readonly StringName DisconnectFromPhoton = "disconnect_from_photon";
        public static readonly StringName JoinRoom = "join_room";
        public static readonly StringName JoinOrCreateRoom = "join_or_create_room";
        public static readonly StringName CreateRoom = "create_room";
        public static readonly StringName LeaveRoom = "leave_room";
        public static readonly StringName GetRoomList = "get_room_list";
        public static readonly StringName GetRoom = "get_room";
        public static readonly StringName GetConnectionStatus = "get_connection_status";
        public static readonly StringName IsConnectedToPhoton = "is_connected_to_photon";
        public static readonly StringName IsInRoom = "is_in_room";
        public static readonly StringName IsMasterClient = "is_master_client";
        public static readonly StringName GetLocalPlayerId = "get_local_player_id";
        public static readonly StringName GetSimulationMode = "get_simulation_mode";
        public static readonly StringName GetNetworkTime = "get_network_time";
        public static readonly StringName GetRtt = "get_rtt";
        public static readonly StringName InterpolateExponential = "interpolate_exponential";
        public static readonly StringName InterpolateExponentialAngle = "interpolate_exponential_angle";
        public static readonly StringName SetSceneLoadMode = "set_scene_load_mode";
        public static readonly StringName GetSceneLoadMode = "get_scene_load_mode";
        public static readonly StringName SetSceneParent = "set_scene_parent";
        public static readonly StringName GetSceneParent = "get_scene_parent";
        public static readonly StringName SetSceneLoader = "set_scene_loader";
        public static readonly StringName LoadScene = "load_scene";
        public static readonly StringName RegisterCurrentScene = "register_current_scene";
        public static readonly StringName UnloadScene = "unload_scene";
        public static readonly StringName DestroySceneObject = "destroy_scene_object";
        public static readonly StringName NotifySceneReady = "notify_scene_ready";
        public static readonly StringName GetLoadedScenes = "get_loaded_scenes";
        public static readonly StringName DeferredSceneLoad = "_deferred_scene_load";
        public static readonly StringName GetRpcInfo = "get_rpc_info";
        public static readonly StringName GetRpcSender = "get_rpc_sender";
        public static readonly StringName RegisterBroadcastReceiver = "register_broadcast_receiver";
        public static readonly StringName UnregisterBroadcastReceiver = "unregister_broadcast_receiver";
        public static readonly StringName Rpc = "rpc";
        public static readonly StringName RpcTo = "rpc_to";
        public static readonly StringName RpcToPlayer = "rpc_to_player";
        public static readonly StringName OnDebuggerMessage = "_on_debugger_message";
        public static readonly StringName GetMonitorBpsSent = "_get_monitor_bps_sent";
        public static readonly StringName GetMonitorBpsRecv = "_get_monitor_bps_recv";
        public static readonly StringName GetMonitorSyncOutboundUs = "_get_monitor_sync_outbound_us";
        public static readonly StringName GetMonitorSyncInboundUs = "_get_monitor_sync_inbound_us";
        public static readonly StringName GetMonitorServiceUs = "_get_monitor_service_us";
        public static readonly StringName GetMonitorUpdateTotalUs = "_get_monitor_update_total_us";
        public static readonly StringName SetAreaKeys = "set_area_keys";
        public static readonly StringName GetAreaKeys = "get_area_keys";
        public static readonly StringName ClearAreaKeys = "clear_area_keys";
        public static readonly StringName AddUserKey = "add_user_key";
        public static readonly StringName RemoveUserKey = "remove_user_key";
        public static readonly StringName ClearUserKeys = "clear_user_keys";
        public static readonly StringName GetUserKeys = "get_user_keys";
        public static readonly StringName ClearAllInterestKeys = "clear_all_interest_keys";
    }

    /// <summary>Cached StringNames for this class's properties.</summary>
    public class PropertyName : GodotObject.PropertyName
    {
    }

    /// <summary>Cached StringNames for this class's signals.</summary>
    public class SignalName : GodotObject.SignalName
    {
        public static readonly StringName RoomJoined = "room_joined";
        public static readonly StringName RoomLeft = "room_left";
        public static readonly StringName RoomListUpdated = "room_list_updated";
        public static readonly StringName ConnectedToPhoton = "connected_to_photon";
        public static readonly StringName ConnectionFailed = "connection_failed";
        public static readonly StringName ConnectionStatusChanged = "connection_status_changed";
        public static readonly StringName SceneLoadRequested = "scene_load_requested";
        public static readonly StringName SceneReady = "scene_ready";
        public static readonly StringName SceneUnloaded = "scene_unloaded";
        public static readonly StringName PlayerLeft = "player_left";
        public static readonly StringName PlayerJoined = "player_joined";
        public static readonly StringName MasterClientChanged = "master_client_changed";
        public static readonly StringName InterestEnter = "interest_enter";
        public static readonly StringName InterestExit = "interest_exit";
    }
}
