using System.Collections.Generic;
using UnityEngine;

namespace Frolics.Pooling {
	public class GameObjectPool {
		private readonly Dictionary<GameObject, Stack<GameObject>> poolDictionary;
		private readonly Dictionary<GameObject, Stack<GameObject>> spawnedObjectDictionary;
		private readonly Transform parent;

		public GameObjectPool(Transform parent) {
			this.poolDictionary = new Dictionary<GameObject, Stack<GameObject>>();
			this.spawnedObjectDictionary = new Dictionary<GameObject, Stack<GameObject>>();
			this.parent = parent;
		}

		private GameObject AddObject(GameObject prefab, Stack<GameObject> pool) {
			GameObject pooledObject = Object.Instantiate(prefab, parent, true);
			pooledObject.gameObject.SetActive(false);
			pooledObject.name = prefab.name;
			spawnedObjectDictionary.Add(pooledObject, pool);

			return pooledObject;
		}

		private GameObject GetObject(GameObject prefab) {
			if (poolDictionary.TryGetValue(prefab, out Stack<GameObject> pool))
				return pool.Count > 0 ? pool.Pop() : AddObject(prefab, pool);

			poolDictionary.Add(prefab, new Stack<GameObject>());
			return AddObject(prefab, poolDictionary[prefab]);
		}

		// NOTE Too many overloads
		public GameObject Spawn(GameObject prefab, Vector3 position) {
			GameObject spawnedObject = GetObject(prefab);
			spawnedObject.transform.SetPositionAndRotation(position, Quaternion.identity);
			spawnedObject.SetActive(true);
			return spawnedObject;
		}

		public GameObject Spawn(GameObject prefab) {
			return Spawn(prefab, Vector3.zero);
		}

		public GameObject Spawn(GameObject prefab, Transform parent) {
			GameObject spawnedObject = Spawn(prefab);
			spawnedObject.transform.SetParent(parent);
			return spawnedObject;
		}

		public GameObject Spawn(GameObject prefab, Transform parent, Vector3 position) {
			GameObject spawnedObject = Spawn(prefab);
			spawnedObject.transform.SetParent(parent);
			spawnedObject.transform.SetPositionAndRotation(position, Quaternion.identity);
			return spawnedObject;
		}

		public void Despawn(GameObject spawnedObject) {
			spawnedObject.SetActive(false);
			spawnedObject.transform.SetParent(parent);

			if (spawnedObjectDictionary.TryGetValue(spawnedObject, out Stack<GameObject> pool))
				pool.Push(spawnedObject);
			else
				Debug.LogWarning("Trying to despawn an object that is not in the pool.");
		}
	}
}
