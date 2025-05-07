using Core.DataTransfer.Definitions.PuzzleLevels;
using Core.PuzzleElements;
using UnityEngine;

namespace Core.PuzzleLevels.Targets {
	public class ScoreTarget : Target {
		private const int BaseScorePerElement = 10;
		private const int MultiplierThreshold = 3;
		private const float MultiplierIncrement = 0.20f;

		private readonly int targetScore;
		private int currentScore;

		public ScoreTarget(ScoreTargetDTO targetDTO) {
			this.targetScore = targetDTO.GetTargetScore();
		}

		public override bool IsTargetCompleted() => currentScore >= targetScore;
		public void IncreaseCurrentScore(Link link) => currentScore += CalculateScore(link);

		private int CalculateScore(Link link) {
			HashList<PuzzleElement> elements = link.GetElements();
			int multiplierAmount = Mathf.Min(0, elements.Count / MultiplierThreshold - 1);
			float multiplier = 1f + MultiplierIncrement * multiplierAmount;
			int scorePerElement = Mathf.RoundToInt(BaseScorePerElement * multiplier);
			return scorePerElement * elements.Count;
		}

		public int GetTargetScore() => targetScore;
		public int GetCurrentScore() => currentScore;
	}
}
