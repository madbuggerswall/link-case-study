using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Contexts {
	[DefaultExecutionOrder(-32)]
	public abstract class SceneContext : MonoBehaviour {
		private static SceneContext instance;

		private readonly Dictionary<System.Type, IInitializable> contextItems = new();

		private void Awake() {
			//  Migrate this line to somewhere relevant
			Application.targetFrameRate = 60;

			AssertSingleton();
			ResolveContext();
			InitializeContext();
			OnInitialized();
		}

		protected abstract void ResolveContext();
		protected abstract void OnInitialized();

		private void InitializeContext() {
			//.NET Standard 2.1 preserves dictionary insertion order which is vital
			foreach ((Type type, IInitializable initializable) in contextItems)
				initializable.Initialize();
		}

		public T Get<T>() where T : class, IInitializable {
			if (contextItems.TryGetValue(typeof(T), out IInitializable contextItem))
				return contextItem as T;

			// This can be an exception as dependent systems would be broken already
			Debug.LogError($"Context item {typeof(T)} cannot be found on current context");
			return null;
		}

		protected void Resolve<T>() where T : IInitializable, new() {
			if (typeof(MonoBehaviour).IsAssignableFrom(typeof(T))) {
				ResolveMono<T>();
			} else {
				ResolvePlain<T>();
			}
		}

		private void ResolveMono<T>() where T : IInitializable {
			T dependency = GetComponentInChildren<T>(true);

			if (dependency is null)
				throw new Exception("Dependency not found: " + typeof(T));

			// Try adding dependency to context dictionary
			if (contextItems.TryAdd(typeof(T), dependency))
				return;

			// This can be an exception as dependent systems would be broken already
			Debug.LogWarning($"Dependency {typeof(T)} is already added to context");
		}

		private void ResolvePlain<T>() where T : IInitializable, new() {
			T dependency = new T();
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
