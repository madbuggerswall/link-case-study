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
		private ScoreManager scoreManager;
		private TargetManager targetManager;

		public void Initialize() {
			levelInitializer = SceneContext.GetInstance().Get<PuzzleLevelInitializer>();
			viewController = SceneContext.GetInstance().Get<PuzzleLevelViewController>();
			chipDefinitionManager = SceneContext.GetInstance().Get<ChipDefinitionManager>();

			puzzleGrid = levelInitializer.GetPuzzleGrid();

			fallManager = new FallManager(this);
			fillManager = new FillManager(this);
			shuffleManager = new ShuffleManager(this);

			turnManager = new TurnManager();
			scoreManager = new ScoreManager();
			targetManager = new TargetManager();

			SignalBus.GetInstance().SubscribeTo<ElementExplodedSignal>(OnElementExploded);
			SignalBus.GetInstance().SubscribeTo<ContextInitializedSignal>(OnContextInitialized);
		}
		
		private void OnElementExploded(ElementExplodedSignal signal) {
			PuzzleElement puzzleElement = signal.PuzzleElement;
			viewController.DespawnElementBehaviour(puzzleElement);
		}

		private void OnContextInitialized(ContextInitializedSignal signal) {
			TryShuffle();
		}

		private void TryShuffle() {
			if (!shuffleManager.IsShuffleNeeded()) {
				return;
			}

			shuffleManager.Shuffle();
			viewController.ShuffleViewHelper.MoveShuffledElements();
		}


		public void Explode(ExplodeLinkCommand command, Link link) {
			if (!link.IsValid(puzzleGrid)) {
				command.InvokeCompletionHandlers();
				return;
			}

			link.Explode(puzzleGrid);

			fallManager.ApplyFall();
			fillManager.ApplyFill();

			HashSet<PuzzleElement> fallenElements = fallManager.GetFallenElements();
			viewController.FallViewHelper.MoveFallenElements(fallenElements);

			HashSet<PuzzleElement> filledElements = fillManager.GetFilledElements();
			viewController.FillViewHelper.MoveFilledElements(filledElements);

			viewController.ViewReadyNotifier.OnReadyForShuffle.AddListener(OnReadyForShuffle);
			viewController.ViewReadyNotifier.OnViewReady.AddListener(command.InvokeCompletionHandlers);
		}

		private void OnReadyForShuffle() {
			if (!shuffleManager.IsShuffleNeeded()) {
				viewController.ViewReadyNotifier.OnShuffleTweensComplete();
				return;
			}

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
		private Target[] targets;

		public void InitializeTargets(PuzzleLevelDefinition levelDefinition) {
			TargetDTO[] targetDTOs = levelDefinition.GetGoals();
			this.targets = new Target[targetDTOs.Length];

			for (int i = 0; i < targetDTOs.Length; i++) {
				TargetDTO targetDTO = targetDTOs[i];
				targets[i] = targetDTO.CreateTarget();
			}
		}
	}

	public class TurnManager {
		private int maxMoveCount;
		private int currentMoveCount;

		public void InitializeMoveCount(PuzzleLevelDefinition levelDefinition) {
			this.maxMoveCount = levelDefinition.GetMaxMoveCount();
		}

		public void OnTurnMade() {
			currentMoveCount++;
		}
	}

	public class ScoreManager {
		private const int BaseScorePerElement = 10;
		private const int MultiplierThreshold = 3;
		private const float MultiplierIncrement = 0.20f;

		private int CalculateScore(Link link) {
			HashList<PuzzleElement> elements = link.GetElements();
			int multiplierAmount = Mathf.Min(0, elements.Count / MultiplierThreshold - 1);
			float multiplier = 1f + MultiplierIncrement * multiplierAmount;
			int scorePerElement = Mathf.RoundToInt(BaseScorePerElement * multiplier);
			return scorePerElement * elements.Count;
		}
	}
}
