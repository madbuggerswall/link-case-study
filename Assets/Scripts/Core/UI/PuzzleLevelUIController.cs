using Core.Contexts;
using Core.PuzzleLevels.Targets;
using Core.PuzzleLevels.Turns;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Core.UI {
	public class PuzzleLevelUIController : MonoBehaviour, IInitializable {
		[SerializeField] private LevelEndPanel levelSuccessPanel;
		[SerializeField] private LevelEndPanel levelFailPanel;
		[SerializeField] private ElementTargetsPanel elementTargetsPanel;
		[SerializeField] private ScoreTargetPanel scoreTargetPanel;
		[SerializeField] private RemainingTurnsPanel remainingTurnsPanel;
		[SerializeField] private Button restartButton;

		// Dependencies
		private TargetManager targetManager;
		private TurnManager turnManager;

		public void Initialize() {
			targetManager = SceneContext.GetInstance().Get<TargetManager>();
			turnManager = SceneContext.GetInstance().Get<TurnManager>();

			elementTargetsPanel.Initialize(targetManager.GetElementTargets());
			scoreTargetPanel.UpdateRemainingScore(targetManager.GetScoreTarget());
			remainingTurnsPanel.UpdateRemainingTurns(turnManager.GetRemainingTurnCount());

			restartButton.onClick.AddListener(OnRestartButtonClick);
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

		public void UpdateScoreTargetView(ScoreTarget target) {
			scoreTargetPanel.UpdateRemainingScore(target);
		}

		public void UpdateRemainingTurnsPanel(int remainingMoves) {
			remainingTurnsPanel.UpdateRemainingTurns(remainingMoves);
		}

		private void OnRestartButtonClick() {
			Scene activeScene = SceneManager.GetActiveScene();
			SceneManager.LoadScene(activeScene.buildIndex);
		}
	}
}
