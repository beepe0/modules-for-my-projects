﻿using UnityEngine;

namespace Singleton
{
    public abstract class Singleton<T> : Singleton where T : MonoBehaviour
    {
        [SerializeField, Header("Singleton")] private bool _dontDestroyOnLoad = true;

        private static T _instance;
        private static readonly object _lock = new object();
        
        public static T Instance
        {
            get
            {
                if (Quitting) return null;
            
                lock (_lock)
                {
                    if (_instance != null) return _instance;
                    
                    var instances = FindObjectsOfType<T>();
                    int count = instances.Length;
                
                    if (count > 0)
                    {
                        if (count == 1) return _instance = instances[0];
                        for (var i = 1; i < instances.Length; i++) Destroy(instances[i]);
                        return _instance = instances[0];
                    }

                    return _instance = new GameObject($"{typeof(T)}").AddComponent<T>();
                }
            }
        }
    
        private void Awake()
        {
            if (_dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
            OnAwake();
        }

        protected virtual void OnAwake() { }
    }

    public abstract class Singleton : MonoBehaviour
    {
        public static bool Quitting { get; private set; }

        private void OnApplicationQuit()
        {
            Quitting = true;
        }
    }
}