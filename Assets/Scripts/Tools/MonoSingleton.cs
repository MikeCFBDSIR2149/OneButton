using UnityEngine;

namespace Tools
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static bool _isExiting;

        public static T Instance
        {
            get
            {
                if (_isExiting)
                {
                    // Debug.LogWarning("[MonoSingleton] Instance is being destroyed, returning null.");
                    return null;
                }
                
                if (_instance) return _instance;
                
                _instance = (T)FindFirstObjectByType(typeof(T));

                if (FindObjectsByType(typeof(T), FindObjectsSortMode.None).Length > 1)
                {
                    Debug.LogError("[MonoSingleton] Something went really wrong " +
                                   " - there should never be more than 1 singleton!" +
                                   " Reopening the scene might fix it.");
                    return _instance;
                }

                if (_instance) return _instance;
                
                GameObject singleton = new GameObject();
                _instance = singleton.AddComponent<T>();
                singleton.name = "(singleton) " + typeof(T).ToString();

                DontDestroyOnLoad(singleton);

                return _instance;
            }
        }
        
        private void OnApplicationQuit()
        {
            _isExiting = true;
        }
    }
}
