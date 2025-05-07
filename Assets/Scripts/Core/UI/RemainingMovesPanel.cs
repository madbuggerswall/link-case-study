using TMPro;
using UnityEngine;

namespace Core.UI {
	public class RemainingMovesPanel : MonoBehaviour {
		[SerializeField] private TextMeshProUGUI remainingMovesText;


		public void UpdateRemainingMoves(int remainingMoves) {
			remainingMovesText.text = remainingMoves.ToString();
		}
	}
}
