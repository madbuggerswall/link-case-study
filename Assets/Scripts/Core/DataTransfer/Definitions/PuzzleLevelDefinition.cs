using UnityEngine;

namespace Core.DataTransfer.Definitions {
	[CreateAssetMenu(fileName = Filename, menuName = MenuName)]
	public class PuzzleLevelDefinition : ScriptableObject {
		private const string Filename = nameof(PuzzleLevelDefinition);
		private const string MenuName = "ScriptableObjects/Definitions/" + Filename;

		[SerializeField] private Vector2Int gridSize;
		[SerializeField] private int maxMoveCount;
		[SerializeField] private GoalDTO[] goals;
		[SerializeField] private ElementPlacementDTO[] elementPlacements;

		public Vector2Int GetGridSize() => gridSize;
		public int GetMaxMoveCount() => maxMoveCount;
		public GoalDTO[] GetGoals() => goals;
		public ElementPlacementDTO[] GetElementPlacements() => elementPlacements;
	}

	[System.Serializable]
	public class GoalDTO {
		[SerializeField] private PuzzleElementDefinition puzzleElementDefinition;
		[SerializeField] private int amount;

		public PuzzleElementDefinition GetPuzzleElementDefinition() => puzzleElementDefinition;
		public int GetAmount() => amount;
	}

	[System.Serializable]
	public class ElementPlacementDTO {
		[SerializeField] private PuzzleElementDefinition puzzleElementDefinition;
		[SerializeField] private int positionIndex;

		public PuzzleElementDefinition GetPuzzleElementDefinition() => puzzleElementDefinition;
		public int GetPositionIndex() => positionIndex;
	}
}