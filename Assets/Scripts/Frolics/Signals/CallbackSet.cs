using System;
using System.Collections.Generic;

namespace Frolics.Signals {
	public class CallbackSet<T> where T : Signal {
		private readonly HashSet<Action<T>> callbacks = new();

		public void Add(Action<T> callback) {
			callbacks.Add(callback);
		}

		public void Remove(Action<T> callback) {
			callbacks.Remove(callback);
		}

		public void Clear() {
			callbacks.Clear();
		}

		public void Invoke(T signal) {
			foreach (Action<T> callback in callbacks) {
				callback.Invoke(signal);
			}
		}
	}
}
