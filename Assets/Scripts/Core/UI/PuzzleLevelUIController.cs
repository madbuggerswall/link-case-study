using Core.Contexts;
using Core.PuzzleLevels;
using Core.PuzzleLevels.Targets;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.UI {
	public class PuzzleLevelUIController : MonoBehaviour, IInitializable {
		[SerializeField] private LevelEndPanel levelSuccessPanel;
		[SerializeField] private LevelEndPanel levelFailPanel;
		[SerializeField] private ElementTargetsPanel elementTargetsPanel;
		[SerializeField] private ScoreTargetPanel scoreTargetPanel;
		[SerializeField] private RemainingTurnsPanel remainingTurnsPanel;

		// Dependencies
		private TargetManager targetManager;
		private TurnManager turnManager;

		public void Initialize() {
			targetManager = SceneContext.GetInstance().Get<TargetManager>();
			turnManager = SceneContext.GetInstance().Get<TurnManager>();

			elementTargetsPanel.Initialize(targetManager.GetElementTargets());
			scoreTargetPanel.UpdateRemainingScore(targetManager.GetScoreTarget());
			remainingTurnsPanel.UpdateRemainingTurns(turnManager.GetRemainingTurnCount());
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
	}
}
