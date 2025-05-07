using Core.PuzzleLevels.Targets;
using TMPro;
using UnityEngine;

namespace Core.UI {
	public class ScoreTargetPanel : MonoBehaviour {
		[SerializeField] private TextMeshProUGUI remainingScoreText;

		public void UpdateRemainingScore(ScoreTarget target) {
			int remainingScore = Mathf.Max(target.GetTargetScore() - target.GetCurrentScore(), 0);
			remainingScoreText.text = remainingScore.ToString();
		}
	}
}
