using System.Collections;
using System.Collections.Generic;

namespace Core {
	public class HashList<T> : IEnumerable<T> {
		private readonly HashSet<T> hashSet = new();
		private readonly List<T> list = new();

		public int Count => list.Count;

		public bool TryAdd(T item) {
			if (!hashSet.Add(item))
				return false;

			list.Add(item);
			return true;
		}

		public bool TryRemove(T item) {
			if (!hashSet.Remove(item))
				return false;

			list.Remove(item);
			return true;
		}

		public void Clear() {
			hashSet.Clear();
			list.Clear();
		}


		public IEnumerator<T> GetEnumerator() => list.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public int IndexOf(T item) => list.IndexOf(item);
		public bool Contains(T item) => hashSet.Contains(item);

		public T this[int index] => list[index];
	}
}
