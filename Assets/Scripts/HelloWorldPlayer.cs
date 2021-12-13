using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace HelloWorld
{
    public class HelloWorldPlayer : NetworkBehaviour
    {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                Move();
            }
        }

        public void Move()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                var randomPosition = GetRandomPositionOnPlane();
                transform.position = randomPosition;
                Position.Value = randomPosition;
            }
            else
            {
                SubmitPositionRequestServerRpc();
            }
        }

        [ServerRpc]
        void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value = GetRandomPositionOnPlane();
        }

        static Vector3 GetRandomPositionOnPlane()
        {
            return new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f));
        }

        private NavMeshAgent _agent;
        private void Start()
        {
            TryGetComponent(out _agent);
        }

        void Update()
        {
            // transform.position = Position.Value;
        }
        
        [ServerRpc]
        private void SubmitCursorPosServerRpc(Vector3 pos)
        {
            SetDestination(pos);
        }

        public void PlayerMove(Vector3 des)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                SetDestination(des);
            }
            else
            {
                SubmitCursorPosServerRpc(des);
            }
        }

        private void SetDestination(Vector3 des)
        {
            if (_agent)
            {
                Debug.Log(des);
                _agent.SetDestination(des);
            }
        }
    }
}