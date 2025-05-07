using System.Collections.Generic;
using Core.Commands;
using Core.Contexts;
using Core.DataTransfer.Definitions.PuzzleElements;
using Core.DataTransfer.Definitions.PuzzleLevels;
using Core.PuzzleElements;
using Core.PuzzleElements.Behaviours;
using Core.PuzzleGrids;
using Core.PuzzleLevels.Targets;
using Frolics.Pooling;
using Frolics.Signals;
using UnityEngine;
using UnityEngine.Events;

// NOTE Rename this namespace to PuzzleMechanics/LevelMechanics/Mechanics
namespace Core.PuzzleLevels {
	public class PuzzleLevelManager : IInitializable {
		// Dependencies
		private PuzzleLevelInitializer levelInitializer;
		private PuzzleLevelViewController viewController;
		private ChipDefinitionManager chipDefinitionManager;

		// Fields
		private PuzzleGrid puzzleGrid;

		private FallManager fallManager;
		private FillManager fillManager;
		private ShuffleManager shuffleManager;

		private TurnManager turnManager;
		private TargetManager targetManager;

		public void Initialize() {
			levelInitializer = SceneContext.GetInstance().Get<PuzzleLevelInitializer>();
			viewController = SceneContext.GetInstance().Get<PuzzleLevelViewController>();
			chipDefinitionManager = SceneContext.GetInstance().Get<ChipDefinitionManager>();

			puzzleGrid = levelInitializer.GetPuzzleGrid();

			fallManager = new FallManager(this);
			fillManager = new FillManager(this);
			shuffleManager = new ShuffleManager(this);

			targetManager = new TargetManager(levelInitializer.GetElementTargets(), levelInitializer.GetScoreTarget());
			turnManager = new TurnManager(levelInitializer.GetMaxMoveCount());

			SignalBus.GetInstance().SubscribeTo<ElementExplodedSignal>(OnElementExploded);
			SignalBus.GetInstance().SubscribeTo<ContextInitializedSignal>(OnContextInitialized);
		}

		private void OnElementExploded(ElementExplodedSignal signal) {
			PuzzleElement puzzleElement = signal.PuzzleElement;
			viewController.DespawnElementBehaviour(puzzleElement);
		}

		private void OnContextInitialized(ContextInitializedSignal signal) {
			if (!shuffleManager.IsShuffleNeeded())
				return;

			shuffleManager.Shuffle();
			viewController.ShuffleViewHelper.MoveShuffledElements();
		}


		public void Explode(ExplodeLinkCommand command, Link link) {
			if (!link.IsValid(puzzleGrid)) {
				command.InvokeCompletionHandlers();
				return;
			}

			link.Explode(puzzleGrid);
			turnManager.OnTurnMade();
			targetManager.CheckForElementTargets(link);
			targetManager.CheckForScoreTarget(link);

			fallManager.ApplyFall();
			fillManager.ApplyFill();

			HashSet<PuzzleElement> fallenElements = fallManager.GetFallenElements();
			HashSet<PuzzleElement> filledElements = fillManager.GetFilledElements();

			viewController.FallViewHelper.MoveFallenElements(fallenElements);
			viewController.ViewReadyNotifier.WaitForFallTweens();

			viewController.FillViewHelper.MoveFilledElements(filledElements);
			viewController.ViewReadyNotifier.WaitForFillTweens();

			if (shuffleManager.IsShuffleNeeded()) {
				viewController.ViewReadyNotifier.OnReadyForShuffle.AddListener(OnReadyForShuffle);
				viewController.ViewReadyNotifier.WaitShuffleForTweens();
			}

			viewController.ViewReadyNotifier.OnViewReady.AddListener(command.InvokeCompletionHandlers);
		}

		private void OnReadyForShuffle() {
			shuffleManager.Shuffle();
			viewController.ShuffleViewHelper.MoveShuffledElements();
		}

		public ColorChip CreateRandomColorChip() {
			ColorChipDefinition definition = chipDefinitionManager.GetRandomColorChipDefinition();
			return new ColorChip(definition);
		}

		// Getters
		public PuzzleGrid GetPuzzleGrid() => puzzleGrid;
	}

	public class TargetManager {
		private readonly PuzzleElementTarget[] elementTargets;
		private readonly ScoreTarget scoreTarget;

		public TargetManager(PuzzleElementTarget[] elementTargets, ScoreTarget scoreTarget) {
			this.elementTargets = elementTargets;
			this.scoreTarget = scoreTarget;
		}

		public void CheckForElementTargets(Link link) {
			PuzzleElementDefinition elementDefinition = link.GetElementDefinition();

			for (int i = 0; i < elementTargets.Length; i++) {
				PuzzleElementTarget target = elementTargets[i];
				PuzzleElementDefinition targetDefinition = target.GetElementDefinition();
				PuzzleElementDefinition linkDefinition = link.GetElementDefinition();
				if (targetDefinition != linkDefinition)
					continue;

				target.IncreaseCurrentAmount(link.GetElements().Count);
			}
		}

		public void CheckForScoreTarget(Link link) {
			scoreTarget.IncreaseCurrentScore(link);
		}

		public bool IsAllTargetsCompleted() {
			for (int i = 0; i < elementTargets.Length; i++)
				if (!elementTargets[i].IsTargetCompleted())
					return false;

			return scoreTarget.IsTargetCompleted();
		}
	}

	public class TurnManager {
		private readonly int maxMoveCount;
		private int currentMoveCount;

		public TurnManager(int maxMoveCount) {
			this.maxMoveCount = maxMoveCount;
		}

		public void OnTurnMade() {
			currentMoveCount++;
		}

		public bool IsTurnsLeft() {
			return maxMoveCount - currentMoveCount > 0;
		}
	}
}
