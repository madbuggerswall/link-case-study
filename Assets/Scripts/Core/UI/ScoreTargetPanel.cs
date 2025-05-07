using TMPro;
using UnityEngine;

namespace Core.UI {
	public class ScoreTargetPanel : MonoBehaviour {
		[SerializeField] private TextMeshProUGUI remainingScoreText;

		public void UpdateRemainingScore(int remainingScore) {
			remainingScoreText.text = remainingScore.ToString();
		}
	}
}
