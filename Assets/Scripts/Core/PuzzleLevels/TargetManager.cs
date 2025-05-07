using Core.Contexts;
using Core.DataTransfer.Definitions.PuzzleElements;
using Core.PuzzleLevels.Targets;
using Core.UI;

namespace Core.PuzzleLevels {
	public class TargetManager : IInitializable {
		private PuzzleElementTarget[] elementTargets;
		private ScoreTarget scoreTarget;

		// Dependencies
		private PuzzleLevelInitializer levelInitializer;
		private PuzzleLevelUIController uiController;

		public void Initialize() {
			this.levelInitializer = SceneContext.GetInstance().Get<PuzzleLevelInitializer>();
			this.uiController = SceneContext.GetInstance().Get<PuzzleLevelUIController>();

			this.elementTargets = levelInitializer.GetElementTargets();
			this.scoreTarget = levelInitializer.GetScoreTarget();
		}

		public void CheckForElementTargets(Link link) {
			PuzzleElementDefinition linkDefinition = link.GetElementDefinition();

			for (int i = 0; i < elementTargets.Length; i++) {
				PuzzleElementTarget target = elementTargets[i];
				if (target.GetElementDefinition() != linkDefinition)
					continue;

				target.IncreaseCurrentAmount(link.GetElements().Count);
				uiController.UpdateElementTargetView(target);
			}
		}

		public void CheckForScoreTarget(Link link) {
			scoreTarget.IncreaseCurrentScore(link);
			uiController.UpdateScoreTargetView(scoreTarget);
		}

		public bool IsAllTargetsCompleted() {
			for (int i = 0; i < elementTargets.Length; i++)
				if (!elementTargets[i].IsTargetCompleted())
					return false;

			return scoreTarget.IsTargetCompleted();
		}

		public PuzzleElementTarget[] GetElementTargets() => elementTargets;
		public ScoreTarget GetScoreTarget() => scoreTarget;
	}
}
