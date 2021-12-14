using System;
using System.Collections;
using System.Collections.Generic;
using Bolt;
using HelloWorld;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour, IMixedRealityInputHandler
{
    private NetworkObject _networkObject;
    private PlayerController _player;
    
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
    public Vector3 replicatedPos = new Vector3();
    
    private void Start()
    {
        _networkObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
        _networkObject.TryGetComponent(out _player);
    }
    void Update()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            Position.Value = transform.position;
            replicatedPos = Position.Value;
        }
        else
        {
            replicatedPos = Position.Value;
        }
    }

    private void OnEnable()
    {
        CoreServices.InputSystem?.RegisterHandler<IMixedRealityInputHandler>(this);
    }
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            Debug.Log("OnNetworkSpawn local client id:" + NetworkManager.Singleton.LocalClientId);
        }
    }

    public void OnInputUp(InputEventData eventData)
    {
        
    }

    public void OnInputDown(InputEventData eventData)
    {
        if (_player != null && eventData.InputSource.Pointers[0].BaseCursor!= null)
        {
            CustomEvent.Trigger(_player.gameObject,"SetCursorDes", eventData.InputSource.Pointers[0].BaseCursor
                .Position);
        }
    }
    
    
    [ServerRpc]
    private void SubmitCursorPosServerRpc(Vector3 pos)
    {
        CustomEvent.Trigger(gameObject, "SetDestination", pos);
    }

    public void BoltSubmitCursorPosToServer(Vector3 pos)
    {
        SubmitCursorPosServerRpc(pos);
    }
        
    public bool IsServer()
    {
        return NetworkManager.Singleton.IsServer;
    }
    
    private void OnDisable()
    {
        CoreServices.InputSystem?.UnregisterHandler<IMixedRealityInputHandler>(this);
    }
}
