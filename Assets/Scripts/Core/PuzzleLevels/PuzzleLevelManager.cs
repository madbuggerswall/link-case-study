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

namespace Core.PuzzleLevels {
	public class PuzzleLevelManager : IInitializable {
		// Dependencies
		private PuzzleLevelInitializer levelInitializer;
		private PuzzleLevelViewController viewController;
		private ObjectPool objectPool;

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
			objectPool = SceneContext.GetInstance().Get<ObjectPool>();

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
			PuzzleElementBehaviour elementBehaviour = viewController.GetPuzzleElementBehaviour(puzzleElement);
			objectPool.Despawn(elementBehaviour);
		}

		public void Explode(ExplodeLinkCommand command, Link link) {
			if (!link.IsValid(puzzleGrid)) {
				command.InvokeCompletionHandlers();
				return;
			}

			link.Explode(puzzleGrid);

			fallManager.ApplyFall(puzzleGrid);
			HashSet<PuzzleElement> fallenElements = fallManager.GetFallenElements();
			viewController.MoveFallenElements(fallenElements);
			viewController.OnViewReady.AddListener(command.InvokeCompletionHandlers);
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
		private readonly PuzzleLevelManager puzzleLevelManager;
		private readonly PuzzleGrid puzzleGrid;
		private HashSet<PuzzleElement> fallenElements = new();

		public FallManager(PuzzleLevelManager puzzleLevelManager) {
			this.puzzleLevelManager = puzzleLevelManager;
			this.puzzleGrid = puzzleLevelManager.GetPuzzleGrid();
		}

		public void ApplyFall(PuzzleGrid puzzleGrid) {
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

		private PuzzleCell[] GetEveryCellOfColumn(int columnIndex) {
			Vector2Int gridSize = puzzleGrid.GetGridSizeInCells();
			PuzzleCell[] columnCells = new PuzzleCell[gridSize.y];

			for (int index = 0; index < columnCells.Length; index++) {
				int gridWidth = gridSize.x;
				columnCells[index] = puzzleGrid.GetCell(index * gridWidth + columnIndex);
			}

			return columnCells;
		}

		private bool IsColumnIndexInRange(int columnIndex) {
			Vector2Int gridSize = puzzleGrid.GetGridSizeInCells();
			return columnIndex >= 0 && columnIndex < gridSize.x;
		}

		// Getters
		public HashSet<PuzzleElement> GetFallenElements() => fallenElements;
	}

	public class FillManager {
		private PuzzleLevelManager puzzleLevelManager;
		private PuzzleGrid puzzleGrid;
		private ChipDefinitionManager chipDefinitionManager;

		public FillManager(PuzzleLevelManager puzzleLevelManager) {
			this.puzzleLevelManager = puzzleLevelManager;
			this.puzzleGrid = puzzleLevelManager.GetPuzzleGrid();
		}

		public void ApplyFill(PuzzleGrid puzzleGrid) {
			Vector2Int gridSize = puzzleGrid.GetGridSizeInCells();

			for (int columnIndex = 0; columnIndex < gridSize.x; columnIndex++) {
				for (int rowIndex = 0; rowIndex < gridSize.y; rowIndex++) {
					PuzzleCell columnCell = puzzleGrid.GetCell(rowIndex * gridSize.x + columnIndex);
					if (columnCell.TryGetPuzzleElement(out PuzzleElement puzzleElement))
						continue;

					ColorChipDefinition definition = chipDefinitionManager.GetRandomColorChipDefinition();
					ColorChip colorChip = new ColorChip(definition);
					columnCell.SetPuzzleElement(colorChip);
				}
			}
		}
	}
}
