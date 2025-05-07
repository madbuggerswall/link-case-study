using System.Collections.Generic;
using Core.Commands;
using Core.Contexts;
using Core.DataTransfer.Definitions.PuzzleElements;
using Core.PuzzleElements;
using Core.PuzzleElements.Signals;
using Core.PuzzleGrids;
using Core.PuzzleLevels.LevelView;
using Core.PuzzleLevels.Links;
using Core.PuzzleLevels.MechanicsHelpers;
using Core.PuzzleLevels.Targets;
using Core.PuzzleLevels.Turns;
using Core.UI;
using Frolics.Signals;

namespace Core.PuzzleLevels {
	public class PuzzleLevelManager : IInitializable {
		// Dependencies
		private PuzzleLevelInitializer levelInitializer;
		private PuzzleLevelViewController viewController;
		private PuzzleLevelUIController uiController;
		private ChipDefinitionManager chipDefinitionManager;
		private TurnManager turnManager;
		private TargetManager targetManager;

		// Helpers
		private FallHelper fallHelper;
		private FillHelper fillHelper;
		private ShuffleHelper shuffleHelper;

		// Fields
		private PuzzleGrid puzzleGrid;

		public void Initialize() {
			levelInitializer = SceneContext.GetInstance().Get<PuzzleLevelInitializer>();
			chipDefinitionManager = SceneContext.GetInstance().Get<ChipDefinitionManager>();
			viewController = SceneContext.GetInstance().Get<PuzzleLevelViewController>();
			uiController = SceneContext.GetInstance().Get<PuzzleLevelUIController>();
			turnManager = SceneContext.GetInstance().Get<TurnManager>();
			targetManager = SceneContext.GetInstance().Get<TargetManager>();

			puzzleGrid = levelInitializer.GetPuzzleGrid();

			fallHelper = new FallHelper(this);
			fillHelper = new FillHelper(this);
			shuffleHelper = new ShuffleHelper(this);

			SignalBus.GetInstance().ClearListeners<ElementExplodedSignal>();
			SignalBus.GetInstance().ClearListeners<ContextInitializedSignal>();

			SignalBus.GetInstance().SubscribeTo<ElementExplodedSignal>(OnElementExploded);
			SignalBus.GetInstance().SubscribeTo<ContextInitializedSignal>(OnContextInitialized);
		}


		private void OnElementExploded(ElementExplodedSignal signal) {
			PuzzleElement puzzleElement = signal.PuzzleElement;
			viewController.DespawnElementBehaviour(puzzleElement);
		}

		private void OnContextInitialized(ContextInitializedSignal signal) {
			if (!shuffleHelper.IsShuffleNeeded())
				return;

			shuffleHelper.Shuffle();
			viewController.ShuffleViewHelper.MoveShuffledElements();
		}


		public void Explode(ExplodeLinkCommand command, Link link) {
			if (!link.IsValid(puzzleGrid)) {
				command.InvokeCompletionHandlers();
				return;
			}

			link.Explode(puzzleGrid);
			turnManager.OnTurnMade();
			targetManager.CheckForTargets(link);

			fallHelper.ApplyFall();
			fillHelper.ApplyFill();

			HashSet<PuzzleElement> fallenElements = fallHelper.GetFallenElements();
			HashSet<PuzzleElement> filledElements = fillHelper.GetFilledElements();

			viewController.FallViewHelper.MoveFallenElements(fallenElements);
			viewController.ViewReadyNotifier.WaitForFallTweens();

			viewController.FillViewHelper.MoveFilledElements(filledElements);
			viewController.ViewReadyNotifier.WaitForFillTweens();

			if (shuffleHelper.IsShuffleNeeded()) {
				viewController.ViewReadyNotifier.OnReadyForShuffle.AddListener(OnReadyForShuffle);
				viewController.ViewReadyNotifier.WaitShuffleForTweens();
			}

			viewController.ViewReadyNotifier.OnViewReady.AddListener(command.InvokeCompletionHandlers);
			viewController.ViewReadyNotifier.OnViewReady.AddListener(CheckLevelSuccess);
			viewController.ViewReadyNotifier.OnViewReady.AddListener(CheckLevelFail);
		}

		private void CheckLevelSuccess() {
			if (!targetManager.IsAllTargetsCompleted())
				return;

			uiController.ShowLevelSuccessPanel();
		}

		private void CheckLevelFail() {
			if (turnManager.IsTurnsLeft() || targetManager.IsAllTargetsCompleted())
				return;

			uiController.ShowLevelFailPanel();
		}

		private void OnReadyForShuffle() {
			shuffleHelper.Shuffle();
			viewController.ShuffleViewHelper.MoveShuffledElements();
		}

		public ColorChip CreateRandomColorChip() {
			ColorChipDefinition definition = chipDefinitionManager.GetRandomColorChipDefinition();
			return new ColorChip(definition);
		}

		// Getters
		public PuzzleGrid GetPuzzleGrid() => puzzleGrid;
	}
}
