using UnityEngine;
    public class SceneBasedSingleton<T> : MonoBehaviour where T : Component
    {
        static public T Instance { get; protected set; }

        public virtual void Awake()
        {
            Instance = this as T;
            SingletonAwake();
        }

        public virtual void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
                SingletonOnDestroy();
            }
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        protected virtual void SingletonAwake()
        {

        }

        protected virtual void SingletonOnDestroy()
        {

        }
    }

