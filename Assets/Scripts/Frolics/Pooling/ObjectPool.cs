using Core.Contexts;
using UnityEngine;

namespace Frolics.Pooling {
	public class ObjectPool : MonoBehaviour, IInitializable {
		private MonoBehaviourPool monoBehaviourPool;
		private GameObjectPool gameObjectPool;

		public void Initialize() {
			monoBehaviourPool = new MonoBehaviourPool(transform);
			gameObjectPool = new GameObjectPool(transform);
		}

		// MonoBehaviourPool
		public T Spawn<T>(T prefab, Vector3 position) where T : MonoBehaviour {
			return monoBehaviourPool.Spawn(prefab, position);
		}

		public T Spawn<T>(T prefab) where T : MonoBehaviour {
			return monoBehaviourPool.Spawn(prefab);
		}

		public T Spawn<T>(T prefab, Transform parent) where T : MonoBehaviour {
			return monoBehaviourPool.Spawn(prefab, parent);
		}

		public T Spawn<T>(T prefab, Transform parent, Vector3 position) where T : MonoBehaviour {
			return monoBehaviourPool.Spawn(prefab, parent, position);
		}

		public void Despawn<T>(T spawnedObject) where T : MonoBehaviour {
			monoBehaviourPool.Despawn(spawnedObject);
		}

		// GameObjectPool
		public GameObject Spawn(GameObject prefab, Vector3 position) {
			return gameObjectPool.Spawn(prefab, position);
		}

		public GameObject Spawn(GameObject prefab) {
			return gameObjectPool.Spawn(prefab);
		}

		public GameObject Spawn(GameObject prefab, Transform parent) {
			return gameObjectPool.Spawn(prefab, parent);
		}

		public GameObject Spawn(GameObject prefab, Transform parent, Vector3 position) {
			return gameObjectPool.Spawn(prefab, parent, position);
		}

		public void Despawn(GameObject spawnedObject) {
			gameObjectPool.Despawn(spawnedObject);
		}
	}
}
