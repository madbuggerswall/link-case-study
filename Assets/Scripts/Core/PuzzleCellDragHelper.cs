using Core.Contexts;
using Core.Input;
using Core.PuzzleElements;
using Core.PuzzleGrids;
using Frolics.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace Core {
	public class PuzzleCellDragHelper : MonoBehaviour {
		private InputController inputController;
		private InputHandler inputHandler;

		private Vector3 pressPosition;
		private Vector3 dragPosition;
		private Vector3 releasePosition;

		private bool isDragging;
		private const float DragThreshold = 0.5f;

		private PuzzleGrid puzzleGrid;
		private readonly HashList<PuzzleCell> puzzleCells = new();
		public HashList<PuzzleCell> PuzzleCells => puzzleCells;

		public UnityEvent OnCellsChanged { get; } = new();
		public UnityEvent OnCellsSelected { get; } = new();


		public void Initialize() {
			SceneContext sceneContext = SceneContext.GetInstance();
			inputController = sceneContext.Get<InputController>();
			puzzleGrid = sceneContext.Get<PuzzleLevelInitializer>().PuzzleGrid;

			inputHandler = inputController.InputHandler;
			inputHandler.PressEvent.AddListener(OnPress);
			inputHandler.ReleaseEvent.AddListener(OnRelease);
		}

		private void Update() {
			dragPosition = inputController.ScreenPositionToWorldSpace(inputHandler.PointerPosition);

			// ApplyDragThreshold();
			if (isDragging)
				OnDrag();
		}

		private void OnPress(PointerPressData pressData) {
			pressPosition = inputController.ScreenPositionToWorldSpace(pressData.PressPosition);
			isDragging = true;
			// if (!puzzleGrid.TryGetPuzzleCell(pressPosition, out PuzzleCell puzzleCell))
			// 	return;
			//
			// puzzleCells.TryAdd(puzzleCell);
		}

		private void OnDrag() {
			if (!puzzleGrid.TryGetPuzzleCell(dragPosition, out PuzzleCell puzzleCell))
				return;

			if (!puzzleCell.TryGetPuzzleElement(out PuzzleElement puzzleElement))
				return;

			if (puzzleCells.Count == 0) {
				puzzleCells.TryAdd(puzzleCell);
				OnCellsChanged.Invoke();
				return;
			}

			PuzzleCell lastAddedCell = puzzleCells[^1];
			if (!IsCellsAdjacent(lastAddedCell, puzzleCell))
				return;

			if (puzzleCells.Count > 1 && puzzleCell == puzzleCells[^2]) {
				puzzleCells.TryRemove(lastAddedCell);
				OnCellsChanged.Invoke();
			}

			lastAddedCell.TryGetPuzzleElement(out PuzzleElement lastAddedElement);
			if (lastAddedElement.GetDefinition() != puzzleElement.GetDefinition())
				return;

			if (puzzleCells.TryAdd(puzzleCell))
				OnCellsChanged.Invoke();
		}

		private void OnRelease(PointerReleaseData releaseData) {
			releasePosition = inputController.ScreenPositionToWorldSpace(releaseData.ReleasePosition);
			pressPosition = releasePosition;

			OnCellsSelected.Invoke();
			puzzleCells.Clear();

			isDragging = false;
		}

		// private void ApplyDragThreshold() {
		// 	float sqrMagnitude = Vector2.SqrMagnitude(pressPosition.GetXY() - dragPosition.GetXY());
		// 	bool aboveDragThreshold = sqrMagnitude > (DragThreshold * DragThreshold);
		// 	if (aboveDragThreshold)
		// 		isDragging = true;
		// }

		private bool IsCellsAdjacent(PuzzleCell centerCell, PuzzleCell cell) {
			PuzzleCell[] cellNeighbors = puzzleGrid.GetNeighbors(centerCell);

			for (int i = 0; i < cellNeighbors.Length; i++)
				if (cell == cellNeighbors[i])
					return true;

			return false;
		}
	}
}
