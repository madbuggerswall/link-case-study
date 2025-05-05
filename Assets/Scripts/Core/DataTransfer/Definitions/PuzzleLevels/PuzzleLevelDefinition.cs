using UnityEngine;

namespace Core.DataTransfer.Definitions.PuzzleLevels {
	[CreateAssetMenu(fileName = Filename, menuName = MenuName)]
	public class PuzzleLevelDefinition : ScriptableObject {
		private const string Filename = nameof(PuzzleLevelDefinition);
		private const string MenuName = "ScriptableObjects/Definitions/" + Filename;

		[SerializeField] private Vector2Int gridSize;
		[SerializeField] private int maxMoveCount;
		[SerializeField] private TargetDTO[] goals;
		[SerializeField] private ElementPlacementDTO[] elementPlacements;

		public Vector2Int GetGridSize() => gridSize;
		public int GetMaxMoveCount() => maxMoveCount;
		public TargetDTO[] GetGoals() => goals;
		public ElementPlacementDTO[] GetElementPlacements() => elementPlacements;
	}
}
