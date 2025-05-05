using System.Collections.Generic;
using Core.Commands;
using Core.Contexts;
using Core.DataTransfer.Definitions.PuzzleElements;
using Core.DataTransfer.Definitions.PuzzleLevels;
using Core.Links;
using Core.PuzzleElements;
using Core.PuzzleElements.Behaviours;
using Core.PuzzleGrids;
using Core.PuzzleLevels.Targets;
using Frolics.Pooling;
using Frolics.Signals;
using UnityEngine;

// NOTE Rename this namespace to PuzzleMechanics/LevelMechanics/Mechanics
namespace Core.PuzzleLevels {
	public class PuzzleLevelManager : IInitializable {
		// Dependencies
		private PuzzleLevelInitializer levelInitializer;
		private PuzzleLevelViewController viewController;
		private ChipDefinitionManager chipDefinitionManager;

		// Fields
		private PuzzleGrid puzzleGrid;

		private TurnManager turnManager;
		private ScoreManager scoreManager;
		private TargetManager targetManager;
		private FallManager fallManager;
		private FillManager fillManager;

		public void Initialize() {
			levelInitializer = SceneContext.GetInstance().Get<PuzzleLevelInitializer>();
			viewController = SceneContext.GetInstance().Get<PuzzleLevelViewController>();
			chipDefinitionManager = SceneContext.GetInstance().Get<ChipDefinitionManager>();

			puzzleGrid = levelInitializer.GetPuzzleGrid();

			turnManager = new TurnManager();
			scoreManager = new ScoreManager();
			targetManager = new TargetManager();
			fallManager = new FallManager(this);
			fillManager = new FillManager(this);

			SignalBus.GetInstance().SubscribeTo<ElementExplodedSignal>(OnElementExploded);
		}

		private void OnElementExploded(ElementExplodedSignal signal) {
			PuzzleElement puzzleElement = signal.PuzzleElement;
			viewController.DespawnElementBehaviour(puzzleElement);
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

			viewController.OnViewReady.AddListener(command.InvokeCompletionHandlers);
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

	public class FallManager {
		private readonly PuzzleLevelManager levelManager;
		private readonly HashSet<PuzzleElement> fallenElements = new();

		public FallManager(PuzzleLevelManager levelManager) {
			this.levelManager = levelManager;
		}

		public void ApplyFall() {
			PuzzleGrid puzzleGrid = levelManager.GetPuzzleGrid();
			Vector2Int gridSize = puzzleGrid.GetGridSizeInCells();
			fallenElements.Clear();

			for (int columnIndex = 0; columnIndex < gridSize.x; columnIndex++) {
				for (int rowIndex = 0; rowIndex < gridSize.y; rowIndex++) {
					PuzzleCell columnCell = puzzleGrid.GetCell(rowIndex * gridSize.x + columnIndex);
					if (!columnCell.TryGetPuzzleElement(out PuzzleElement puzzleElement))
						continue;

					puzzleElement.Fall(puzzleGrid);
					fallenElements.Add(puzzleElement);
				}
			}
		}

		// Getters
		public HashSet<PuzzleElement> GetFallenElements() => fallenElements;
	}

	public class FillManager {
		private readonly PuzzleLevelManager levelManager;
		private readonly HashSet<PuzzleElement> filledElements = new();

		public FillManager(PuzzleLevelManager levelManager) {
			this.levelManager = levelManager;
		}

		public void ApplyFill() {
			PuzzleGrid puzzleGrid = levelManager.GetPuzzleGrid();
			Vector2Int gridSize = puzzleGrid.GetGridSizeInCells();
			filledElements.Clear();

			// Assumes that a fall operation has already resolved empty spaces
			for (int columnIndex = 0; columnIndex < gridSize.x; columnIndex++) {
				for (int rowIndex = 0; rowIndex < gridSize.y; rowIndex++) {
					PuzzleCell columnCell = puzzleGrid.GetCell(rowIndex * gridSize.x + columnIndex);
					if (columnCell.TryGetPuzzleElement(out _))
						continue;

					ColorChip colorChip = levelManager.CreateRandomColorChip();
					columnCell.SetPuzzleElement(colorChip);
					filledElements.Add(colorChip);
				}
			}
		}

		public HashSet<PuzzleElement> GetFilledElements() => filledElements;
	}
}
