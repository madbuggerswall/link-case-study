using TMPro;
using UnityEngine;

namespace Core.UI {
	public class RemainingTurnsPanel : MonoBehaviour {
		[SerializeField] private TextMeshProUGUI remainingMovesText;

		public void UpdateRemainingTurns(int remainingMoves) {
			remainingMovesText.text = remainingMoves.ToString();
		}
	}
}
