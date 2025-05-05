using Core.Commands;
using Core.Contexts;
using Core.Input;
using Core.Links;
using Core.PuzzleElements;
using Core.PuzzleGrids;
using Core.PuzzleLevels;
using UnityEngine;

namespace Core.LinkInput {
	public class LinkInputManager : MonoBehaviour, IInitializable {
		private readonly HashList<PuzzleElement> selectedElements = new();

		private Vector3 pressPosition;
		private Vector3 dragPosition;
		private Vector3 releasePosition;
		private bool isDragging;

		private PuzzleCellDragHelper dragHelper;
		private InputHandler inputHandler;

		// Dependencies
		private InputManager inputManager;
		private CommandInvoker commandInvoker;
		private PuzzleLevelViewController viewController;
		private PuzzleLevelManager levelManager;

		public void Initialize() {
			inputManager = SceneContext.GetInstance().Get<InputManager>();
			commandInvoker = SceneContext.GetInstance().Get<CommandInvoker>();
			viewController = SceneContext.GetInstance().Get<PuzzleLevelViewController>();
			levelManager = SceneContext.GetInstance().Get<PuzzleLevelManager>();

			PuzzleGrid puzzleGrid = levelManager.GetPuzzleGrid();
			dragHelper = new PuzzleCellDragHelper(this, puzzleGrid);

			inputHandler = inputManager.CommonInputHandler;
			inputHandler.PressEvent.AddListener(OnPress);
			inputHandler.ReleaseEvent.AddListener(OnRelease);
		}

		private void Update() {
			dragPosition = inputManager.ScreenPositionToWorldSpace(inputHandler.PointerPosition);
			if (isDragging)
				dragHelper.OnDrag(dragPosition);
		}

		private void OnPress(PointerPressData pressData) {
			pressPosition = inputManager.ScreenPositionToWorldSpace(pressData.PressPosition);
			isDragging = true;
		}

		private void OnRelease(PointerReleaseData releaseData) {
			releasePosition = inputManager.ScreenPositionToWorldSpace(releaseData.ReleasePosition);
			pressPosition = releasePosition;
			isDragging = false;

			dragHelper.OnRelease();
		}


		public void OnCellsSelectionChanged(HashList<PuzzleCell> selectedCells) {
			UpdateSelectedElements(selectedCells);

			viewController.ScaleDownUnselectedElements(selectedElements);
			viewController.ScaleUpSelectedElements(selectedElements);
		}

		public void OnCellSelectionAccepted(HashList<PuzzleCell> selectedCells) {
			UpdateSelectedElements(selectedCells);
			viewController.ResetSelectedElements(selectedElements);
			if (selectedCells.Count == 0)
				return;

			Link link = new(selectedElements);
			ExplodeLinkCommand command = new ExplodeLinkCommand(levelManager, link);
			commandInvoker.Enqueue(command);
		}

		private void UpdateSelectedElements(HashList<PuzzleCell> selectedCells) {
			selectedElements.Clear();

			for (int index = 0; index < selectedCells.Count; index++) {
				PuzzleCell selectedCell = selectedCells[index];

				if (!selectedCell.TryGetPuzzleElement(out PuzzleElement puzzleElement))
					break;

				// NOTE This part seems very similar to DragHelper.OnDrag, maybe it can be accessed by LinkManager
				if (selectedElements.Count == 0)
					selectedElements.TryAdd(puzzleElement);

				if (selectedElements[^1].GetDefinition() != puzzleElement.GetDefinition())
					break;

				selectedElements.TryAdd(puzzleElement);
			}
		}
	}
}
