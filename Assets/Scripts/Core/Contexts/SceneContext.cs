using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Contexts {
	[DefaultExecutionOrder(-32)]
	public abstract class SceneContext : MonoBehaviour {
		private static SceneContext instance;

		// To support vanilla classes IDependency/IContextItem might be needed
		private readonly Dictionary<System.Type, MonoBehaviour> contextItems = new();

		private void Awake() {
			AssertSingleton();
			ResolveContext();
			InitializeContext();
		}

		protected abstract void ResolveContext();
		protected abstract void InitializeContext();

		public T Get<T>() where T : MonoBehaviour {
			if (contextItems.TryGetValue(typeof(T), out MonoBehaviour contextItem))
				return contextItem as T;

			// This can be an exception as dependent systems would be broken already
			Debug.LogWarning($"Context item {typeof(T)} cannot be found on current context");
			return null;
		}

		protected void Resolve<T>() where T : MonoBehaviour {
			T dependency = GetComponentInChildren<T>(true) ?? FindObjectOfType<T>(true);
			dependency?.transform.SetParent(transform);

			if (dependency is null)
				throw new Exception("Dependency not found: " + typeof(T));

			// Try adding dependency to context dictionary
			if (contextItems.TryAdd(typeof(T), dependency))
				return;

			// This can be an exception as dependent systems would be broken already
			Debug.LogWarning($"Dependency {typeof(T)} is already added to context");
		}

		// Singleton Operations
		private void AssertSingleton() {
			if (instance is not null)
				Destroy(instance);

			instance = this;
		}

		public static SceneContext GetInstance() => instance;
	}
}
