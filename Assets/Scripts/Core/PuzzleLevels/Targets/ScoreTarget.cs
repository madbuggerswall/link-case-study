using Core.DataTransfer.Definitions;
using Core.DataTransfer.Definitions.PuzzleLevels;

namespace Core.PuzzleLevels.Targets {
	public class ScoreTarget : Target {
		private readonly int targetScore;
		private int currentScore;

		public ScoreTarget(ScoreTargetDTO targetDTO) {
			this.targetScore = targetDTO.GetTargetScore();
		}
	
		public override bool IsTargetCompleted() => currentScore >= targetScore;
	}
}