using System.Collections.Generic;
using Core.Contexts;
using Core.PuzzleElements;
using Core.PuzzleElements.Behaviours;
using Core.PuzzleGrids;
using UnityEngine;

namespace Core.PuzzleLevels {
	public class PuzzleLevelViewController : MonoBehaviour {
		private readonly Dictionary<PuzzleElement, PuzzleElementBehaviour> elementBehaviours = new();
		private readonly Dictionary<PuzzleGrid, PuzzleGridBehaviour> gridBehaviours = new();
		private readonly Dictionary<PuzzleCell, PuzzleCellBehaviour> cellBehaviours = new();

		// Dependencies
		private PuzzleElementBehaviourFactory elementBehaviourFactory;
		private PuzzleGridBehaviourFactory gridBehaviourFactory;
		private PuzzleCellBehaviourFactory cellBehaviourFactory;
		private PuzzleLevelInitializer levelInitializer;

		private ScaledViewHelper scaledViewHelper;

		public void Initialize() {
			this.elementBehaviourFactory = SceneContext.GetInstance().Get<PuzzleElementBehaviourFactory>();
			this.gridBehaviourFactory = SceneContext.GetInstance().Get<PuzzleGridBehaviourFactory>();
			this.cellBehaviourFactory = SceneContext.GetInstance().Get<PuzzleCellBehaviourFactory>();
			this.levelInitializer = SceneContext.GetInstance().Get<PuzzleLevelInitializer>();

			// PuzzleGridBehaviours
			PuzzleGrid puzzleGrid = levelInitializer.GetPuzzleGrid();
			SpawnGridBehaviour(puzzleGrid);

			// PuzzleCellBehaviours
			PuzzleGridBehaviour gridBehaviour = GetPuzzleGridBehaviour(puzzleGrid);
			SpawnCellBehaviours(gridBehaviour);

			// PuzzleElementBehaviours
			SpawnElements(puzzleGrid);

			scaledViewHelper = new ScaledViewHelper(this);
		}

		// Initializer methods
		private void SpawnGridBehaviour(PuzzleGrid puzzleGrid) {
			PuzzleGridBehaviour gridBehaviour = gridBehaviourFactory.Create(puzzleGrid);
			gridBehaviours.Add(puzzleGrid, gridBehaviour);
		}

		private void SpawnCellBehaviours(PuzzleGridBehaviour puzzleGridBehaviour) {
			PuzzleGrid puzzleGrid = puzzleGridBehaviour.GetPuzzleGrid();
			Transform cellsParent = puzzleGridBehaviour.GetCellsParent();
			PuzzleCell[] puzzleCells = puzzleGrid.GetCells();

			for (int i = 0; i < puzzleCells.Length; i++) {
				PuzzleCell cell = puzzleCells[i];
				PuzzleCellBehaviour cellBehaviour = cellBehaviourFactory.Create(cell, cellsParent);
				cellBehaviours.Add(cell, cellBehaviour);
			}
		}

		private void SpawnElements(PuzzleGrid puzzleGrid) {
			// Puzzle Elements - Fill with random color chips
			PuzzleCell[] puzzleCells = puzzleGrid.GetCells();

			for (int i = 0; i < puzzleCells.Length; i++) {
				PuzzleCell cell = puzzleCells[i];
				if(!cell.TryGetPuzzleElement(out PuzzleElement element))
					return;

				PuzzleElementBehaviour elementBehaviour = elementBehaviourFactory.Create(element, cell);
				elementBehaviour.SetSortingOrder(i);

				elementBehaviours.Add(element, elementBehaviour);
			}
		}
		

		// Helper methods
		public void ScaleUpSelectedElements(HashList<PuzzleElement> puzzleElements) {
			scaledViewHelper.ScaleUpSelectedElements(puzzleElements);
		}

		public void ScaleDownUnselectedElements(HashList<PuzzleElement> puzzleElements) {
			scaledViewHelper.ScaleDownUnselectedElements(puzzleElements);
		}


		// Getters
		public PuzzleGridBehaviour GetPuzzleGridBehaviour(PuzzleGrid puzzleGrid) {
			return gridBehaviours.GetValueOrDefault(puzzleGrid);
		}

		public PuzzleCellBehaviour GetPuzzleCellBehaviour(PuzzleCell puzzleCell) {
			return cellBehaviours.GetValueOrDefault(puzzleCell);
		}

		public PuzzleElementBehaviour GetPuzzleElementBehaviour(PuzzleElement element) {
			return elementBehaviours.GetValueOrDefault(element);
		}
	}
}
