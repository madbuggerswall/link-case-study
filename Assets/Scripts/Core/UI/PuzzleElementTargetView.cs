using Core.PuzzleLevels.Targets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI {
	public class PuzzleElementTargetView : MonoBehaviour {
		[SerializeField] private Image elementImage;
		[SerializeField] private TextMeshProUGUI remainingTargetText;

		public void Initialize(PuzzleElementTarget target) {
			elementImage.sprite = target.GetElementDefinition().GetSprite();
			remainingTargetText.text = target.GetTargetAmount().ToString();
		}

		public void UpdateRemainingAmount(int remainingAmount) {
			remainingTargetText.text = remainingAmount.ToString();
		}
	}
}
