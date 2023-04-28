using Unity.Netcode;
using UnityEngine;

namespace Networking
{
    
    public class SingletonNetworked<T> : NetworkBehaviour where T : NetworkBehaviour
    {
        private static T _instance;
    
        public static T instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
    
                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject();
                        _instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString() + " (Singleton)";
    
                        DontDestroyOnLoad(singletonObject);
                    }
                }
    
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
    
        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    
        private void OnDestroy()
        {
            _instance = null;
        }
    }
}