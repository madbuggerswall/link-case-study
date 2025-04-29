using System.Collections.Generic;
using UnityEngine;

namespace Frolics.Pooling {
	public class MonoBehaviourPool {
		private readonly Dictionary<System.Type, Stack<MonoBehaviour>> poolDictionary;
		private readonly Transform parent;

		public MonoBehaviourPool(Transform parent) {
			this.poolDictionary = new Dictionary<System.Type, Stack<MonoBehaviour>>();
			this.parent = parent;
		}

		private T AddObject<T>(T prefab, Stack<MonoBehaviour> pool) where T : MonoBehaviour {
			T pooledObject = Object.Instantiate(prefab, parent, true);
			pooledObject.name = prefab.name;

			return pooledObject;
		}

		private T GetObject<T>(T prefab) where T : MonoBehaviour {
			if (poolDictionary.TryGetValue(typeof(T), out Stack<MonoBehaviour> pool))
				return pool.Count > 0 ? pool.Pop() as T : AddObject(prefab, pool);

			Stack<MonoBehaviour> createdPool = new();
			poolDictionary.Add(typeof(T), createdPool);
			return AddObject(prefab, createdPool);
		}

		// NOTE Too many overloads
		public T Spawn<T>(T prefab, Vector3 position) where T : MonoBehaviour {
			T spawnedObject = GetObject(prefab);
			spawnedObject.transform.SetPositionAndRotation(position, Quaternion.identity);
			spawnedObject.gameObject.SetActive(true);
			return spawnedObject;
		}

		public T Spawn<T>(T prefab) where T : MonoBehaviour {
			return Spawn(prefab, Vector3.zero);
		}

		public T Spawn<T>(T prefab, Transform parent) where T : MonoBehaviour {
			T spawnedObject = Spawn(prefab);
			spawnedObject.transform.SetParent(parent);
			return spawnedObject;
		}

		public T Spawn<T>(T prefab, Transform parent, Vector3 position) where T : MonoBehaviour {
			T spawnedObject = Spawn(prefab);
			spawnedObject.transform.SetParent(parent);
			spawnedObject.transform.SetPositionAndRotation(position, Quaternion.identity);
			return spawnedObject;
		}

		public void Despawn<T>(T spawnedObject) where T : MonoBehaviour {
			spawnedObject.gameObject.SetActive(false);
			spawnedObject.transform.SetParent(parent);

			if (poolDictionary.TryGetValue(typeof(T), out Stack<MonoBehaviour> pool))
				pool.Push(spawnedObject);
			else {
				Stack<MonoBehaviour> createdPool = new();
				poolDictionary.Add(typeof(T), createdPool);
				createdPool.Push(spawnedObject);
			}
		}
	}
}
