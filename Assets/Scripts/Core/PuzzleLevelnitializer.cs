using System.Collections;
using System.Collections.Generic;
using Core.Contexts;
using Core.DataTransfer.Definitions;
using Core.PuzzleElements;
using Core.PuzzleGrids;
using UnityEngine;

namespace Core {
	// NOTE Rename this to PuzzleLevelManager
	public class PuzzleLevelInitializer : MonoBehaviour {
		[SerializeField] private PuzzleLevelDefinition levelDefinition;

		// Dependencies
		private PuzzleGridBehaviourFactory gridBehaviourFactory;
		private PuzzleCellBehaviourFactory cellBehaviourFactory;
		private PuzzleElementBehaviourFactory elementBehaviourFactory;
		private CameraController cameraController;

		public void Initialize() {
			gridBehaviourFactory = SceneContext.GetInstance().Get<PuzzleGridBehaviourFactory>();
			cellBehaviourFactory = SceneContext.GetInstance().Get<PuzzleCellBehaviourFactory>();
			elementBehaviourFactory = SceneContext.GetInstance().Get<PuzzleElementBehaviourFactory>();
			cameraController = SceneContext.GetInstance().Get<CameraController>();

			// Puzzle Grid
			float cellDiameter = 1f;
			Vector2Int gridSize = levelDefinition.GetGridSize();

			PuzzleGridBehaviour gridBehaviour = gridBehaviourFactory.Create(gridSize, cellDiameter);
			PuzzleGrid puzzleGrid = gridBehaviour.GetPuzzleGrid();
			SpawnCellBehaviours(gridBehaviour);

			// Puzzle Elements - Fill with random color chips
			PuzzleCell[] puzzleCells = puzzleGrid.GetCells();
			for (int i = 0; i < puzzleCells.Length; i++) {
				PuzzleElementBehaviour elementBehaviour = elementBehaviourFactory.CreateRandomColorChip(puzzleCells[i]);
				elementBehaviour.SetSortingOrder(i);
			}

			// Puzzle Elements - Manually placed elements
			ElementPlacementDTO[] elementPlacements = levelDefinition.GetElementPlacements();
			for (int i = 0; i < elementPlacements.Length; i++) {
				int cellIndex = elementPlacements[i].GetPositionIndex();
				PuzzleElementDefinition definition = elementPlacements[i].GetPuzzleElementDefinition();
				PuzzleCell puzzleCell = puzzleCells[cellIndex];

				PuzzleElementBehaviour elementBehaviour = elementBehaviourFactory.Create(definition, puzzleCell);
				elementBehaviour.SetSortingOrder(cellIndex);
			}

			// Camera Controller
			cameraController.CenterCameraToGrid(puzzleGrid);
			cameraController.AdjustOrthographicSizeToFit(puzzleGrid);
		}

		private void SpawnCellBehaviours(PuzzleGridBehaviour puzzleGridBehaviour) {
			PuzzleGrid puzzleGrid = puzzleGridBehaviour.GetPuzzleGrid();
			Transform cellsParent = puzzleGridBehaviour.GetCellsParent();
			PuzzleCell[] puzzleCells = puzzleGrid.GetCells();

			for (int i = 0; i < puzzleCells.Length; i++)
				cellBehaviourFactory.Create(puzzleCells[i], cellsParent);
		}
	}
}

public class MatchManager {
	private readonly PuzzleGrid puzzleGrid;
	private readonly Dictionary<PuzzleElement, PuzzleMatch> matchesByItem;

	public MatchManager(PuzzleGrid puzzleGrid) {
		this.puzzleGrid = puzzleGrid;

		int maxMatchCount = GetMaxMatchCount(puzzleGrid);
		this.matchesByItem = new Dictionary<PuzzleElement, PuzzleMatch>(maxMatchCount);
	}

	private void FindMatches() {
		PuzzleCell[] puzzleCells = puzzleGrid.GetCells();

		for (int i = 0; i < puzzleCells.Length; i++) {
			PuzzleElement currentItem = puzzleCells[i].GetPuzzleElement();
			PuzzleCell[] neighborCells = puzzleGrid.GetNeighbors(puzzleCells[i]);

			for (int j = 0; j < neighborCells.Length; j++) {
				PuzzleElement neighborItem = neighborCells[j].GetPuzzleElement();
				if (currentItem.GetDefinition() != neighborItem.GetDefinition())
					continue;

				if (matchesByItem.TryGetValue(currentItem, out PuzzleMatch formerMatch)) {
					if (formerMatch.Add(neighborItem))
						matchesByItem.Add(neighborItem, formerMatch);
				} else {
					PuzzleMatch puzzleMatch = new(currentItem, neighborItem);
					matchesByItem.Add(currentItem, puzzleMatch);
					matchesByItem.Add(neighborItem, puzzleMatch);
				}
			}
		}
	}

	private static int GetMaxMatchCount(PuzzleGrid puzzleGrid) {
		Vector2Int gridSize = puzzleGrid.GetGridSizeInCells();
		int cellCount = gridSize.x * gridSize.y;
		int maxMatchCount = cellCount / 2;

		return maxMatchCount;
	}
}

public class PuzzleMatch : IEnumerable<PuzzleElement> {
	private readonly HashSet<PuzzleElement> puzzleElements;

	public PuzzleMatch() {
		puzzleElements = new HashSet<PuzzleElement>();
	}

	public PuzzleMatch(PuzzleElement current, PuzzleElement neighbor) {
		puzzleElements = new HashSet<PuzzleElement> { current, neighbor };
	}

	public bool Add(PuzzleElement puzzleElement) {
		return puzzleElements.Add(puzzleElement);
	}

	// Make it iterable
	public IEnumerator<PuzzleElement> GetEnumerator() => puzzleElements.GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
