using Core.PuzzleElements;
using Core.PuzzleGrids;
using UnityEngine;

namespace Core.DataTransfer.Definitions {
	public abstract class PuzzleElementDefinition : ScriptableObject {
		[SerializeField] private PuzzleElementBehaviour prefab;
		public PuzzleElementBehaviour GetPrefab() => prefab;
		public abstract PuzzleElement CreateElement();
	}
}
