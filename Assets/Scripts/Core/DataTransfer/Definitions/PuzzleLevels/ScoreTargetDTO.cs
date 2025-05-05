using Core.PuzzleLevels.Targets;
using UnityEngine;

namespace Core.DataTransfer.Definitions.PuzzleLevels {
	[System.Serializable]
	public class ScoreTargetDTO : TargetDTO {
		[SerializeField] private int targetScore;
		
		public int GetTargetScore() => targetScore;
		public override Target CreateTarget() {
			return new ScoreTarget(this);
		}
	}
}
