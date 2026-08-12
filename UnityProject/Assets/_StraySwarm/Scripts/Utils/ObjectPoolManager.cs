using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace StraySwarm.Utils
{
    /// <summary>
    /// Professional Object Pooling system using Unity 6's UnityEngine.Pool.
    /// Prevents GC (Garbage Collection) spikes on mobile devices by reusing GameObjects!
    /// </summary>
    public class ObjectPoolManager : MonoBehaviour
    {
        public static ObjectPoolManager Instance { get; private set; }

        private Dictionary<GameObject, IObjectPool<GameObject>> _pools = new Dictionary<GameObject, IObjectPool<GameObject>>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            if (prefab == null) return null;

            if (!_pools.ContainsKey(prefab))
            {
                CreatePool(prefab);
            }

            GameObject obj = _pools[prefab].Get();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            return obj;
        }

        public void Release(GameObject prefab, GameObject obj)
        {
            if (prefab != null && _pools.ContainsKey(prefab))
            {
                _pools[prefab].Release(obj);
            }
            else
            {
                Destroy(obj);
            }
        }

        private void CreatePool(GameObject prefab)
        {
            IObjectPool<GameObject> pool = new ObjectPool<GameObject>(
                createFunc: () => Instantiate(prefab),
                actionOnGet: (obj) => obj.SetActive(true),
                actionOnRelease: (obj) => obj.SetActive(false),
                actionOnDestroy: (obj) => Destroy(obj),
                defaultCapacity: 10,
                maxSize: 50
            );

            _pools.Add(prefab, pool);
        }
    }
}
