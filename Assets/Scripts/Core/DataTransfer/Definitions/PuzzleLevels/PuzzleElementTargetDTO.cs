using Core.DataTransfer.Definitions.PuzzleElements;
using Core.PuzzleLevels.Targets;
using UnityEngine;

namespace Core.DataTransfer.Definitions.PuzzleLevels {
	[System.Serializable]
	public class PuzzleElementTargetDTO : TargetDTO {
		[SerializeField] private PuzzleElementDefinition puzzleElementDefinition;
		[SerializeField] private int targetAmount;

		public PuzzleElementDefinition GetTargetDefinition() => puzzleElementDefinition;
		public int GetTargetAmount() => targetAmount;

		public override Target CreateTarget() => new PuzzleElementTarget(this);
	}
}
