using Core.Contexts;
using Core.PuzzleLevels.Targets;
using UnityEngine;

namespace Core.UI {
	public class LevelPlayUIController : MonoBehaviour, IInitializable {
		[SerializeField] private LevelEndPanel levelSuccessPanel;
		[SerializeField] private LevelEndPanel levelFailPanel;
		[SerializeField] private ElementTargetsPanel elementTargetsPanel;
		[SerializeField] private ScoreTargetPanel scoreTargetPanel;
		[SerializeField] private RemainingMovesPanel remainingMovesPanel;

		// Dependencies
		// private PuzzleLevelManager levelManager;

		public void Initialize() {
			// levelManager = SceneContext.GetInstance().Get<PuzzleLevelManager>();
		}

		public void ShowLevelSuccessPanel() {
			levelSuccessPanel.gameObject.SetActive(true);
		}

		public void ShowLevelFailPanel() {
			levelFailPanel.gameObject.SetActive(true);
		}

		public void UpdateElementTargetView(PuzzleElementTarget target) {
			elementTargetsPanel.UpdateTargetView(target);
		}

		public void UpdateScoreTargetView(int remainingScore) {
			scoreTargetPanel.UpdateRemainingScore(remainingScore);
		}

		public void UpdateRemainingMovesPanel(int remainingMoves) {
			remainingMovesPanel.UpdateRemainingMoves(remainingMoves);
		}
	}
}
