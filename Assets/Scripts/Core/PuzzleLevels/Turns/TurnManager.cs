using Core.Contexts;
using Core.UI;

namespace Core.PuzzleLevels.Turns {
	public class TurnManager : IInitializable {
		private int maxTurnCount;
		private int currentTurnCount;

		// Dependencies
		private PuzzleLevelUIController uiController;
		private PuzzleLevelInitializer levelInitializer;

		public void Initialize() {
			this.uiController = SceneContext.GetInstance().Get<PuzzleLevelUIController>();
			this.levelInitializer = SceneContext.GetInstance().Get<PuzzleLevelInitializer>();

			this.maxTurnCount = levelInitializer.GetMaxMoveCount();
		}

		public void OnTurnMade() {
			currentTurnCount++;
			uiController.UpdateRemainingTurnsPanel(GetRemainingTurnCount());
		}

		public bool IsTurnsLeft() => maxTurnCount - currentTurnCount > 0;
		public int GetRemainingTurnCount() => maxTurnCount - currentTurnCount;
	}
}
