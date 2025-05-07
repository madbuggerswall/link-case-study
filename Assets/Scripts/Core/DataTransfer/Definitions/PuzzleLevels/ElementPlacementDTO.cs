using Core.DataTransfer.Definitions.PuzzleElements;
using UnityEngine;

namespace Core.DataTransfer.Definitions.PuzzleLevels {
	[System.Serializable]
	public class ElementPlacementDTO {
		[SerializeField] private PuzzleElementDefinition puzzleElementDefinition;
		[SerializeField] private int positionIndex;

		public PuzzleElementDefinition GetPuzzleElementDefinition() => puzzleElementDefinition;
		public int GetPositionIndex() => positionIndex;
	}
}
