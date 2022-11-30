using UnityEngine;

namespace DesignPattern
{
    public class UnitySingleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType(typeof(T)) as T;
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject(typeof(T).Name);
                        _instance = (T)obj.AddComponent(typeof(T));
                        //obj.hideFlags = HideFlags.DontSave;
                        // obj.hideFlags = HideFlags.HideAndDontSave;
                    }
                }
                return _instance;
            }
        }

        public virtual void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            if (_instance == null)
            {
                _instance = this as T;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
