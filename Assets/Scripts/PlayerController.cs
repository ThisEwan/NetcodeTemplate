using System;
using System.Collections;
using System.Collections.Generic;
using HelloWorld;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : MonoBehaviour, IMixedRealityInputHandler
{
    private void OnEnable()
    {
        CoreServices.InputSystem?.RegisterHandler<IMixedRealityInputHandler>(this);
    }

    private void OnDisable()
    {
        CoreServices.InputSystem?.UnregisterHandler<IMixedRealityInputHandler>(this);
    }

    private NetworkObject _networkObject;
    private HelloWorldPlayer _player;
    private void Start()
    {
        _networkObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
        _networkObject.TryGetComponent(out _player);
    }

    public void OnInputUp(InputEventData eventData)
    {
        
    }

    public void OnInputDown(InputEventData eventData)
    {
        if (_player != null && eventData.InputSource.Pointers[0].BaseCursor!= null)
        {
            _player.PlayerMove(eventData.InputSource.Pointers[0].BaseCursor
                .Position);
        }
    }
}
