using Core.PuzzleElements;
using Core.PuzzleElements.Behaviours;
using UnityEngine;

namespace Core.DataTransfer.Definitions.PuzzleElements {
	public abstract class PuzzleElementDefinition : ScriptableObject {
		[SerializeField] private PuzzleElementBehaviour prefab;

		public abstract PuzzleElement CreateElement();
		public abstract Sprite GetSprite();

		public PuzzleElementBehaviour GetPrefab() => prefab;
	}
}
