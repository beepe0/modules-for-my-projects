using System;
using UnityEngine;

namespace Network.UnityClient.Behaviors
{
    public abstract class UNetworkObjectBehavior<T> : UNetworkObjectBehavior where T : UNetworkClient
    {
        public T connection;

        private void Awake()
        {
            
        }
    }
    
    public abstract class UNetworkObjectBehavior : MonoBehaviour
    {

    }
}