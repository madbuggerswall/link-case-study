using System;
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
		private readonly HashList<PuzzleElement> puzzleElements = new();

		private Vector3 pressPosition;
		private Vector3 dragPosition;
		private Vector3 releasePosition;
		private bool isDragging;
		
		private PuzzleCellDragHelper dragHelper;
		// Dependencies
		private InputController inputController;
		private InputHandler inputHandler;
		private PuzzleLevelViewController viewController;
		private CommandInvoker commandInvoker;
		private LinkManager linkManager;

		public void Initialize() {
			commandInvoker = SceneContext.GetInstance().Get<CommandInvoker>();
			viewController = SceneContext.GetInstance().Get<PuzzleLevelViewController>();
			inputController = SceneContext.GetInstance().Get<InputController>();
			dragHelper = SceneContext.GetInstance().Get<PuzzleCellDragHelper>();
			// TODO Get LinkManager
			
			inputHandler = inputController.InputHandler;
			inputHandler.PressEvent.AddListener(OnPress);
			inputHandler.ReleaseEvent.AddListener(OnRelease);
		}

		public void Update() {
			dragPosition = inputController.ScreenPositionToWorldSpace(inputHandler.PointerPosition);
			if (isDragging)
				dragHelper.OnDrag(dragPosition);
		}

		private void OnPress(PointerPressData pressData) {
			pressPosition = inputController.ScreenPositionToWorldSpace(pressData.PressPosition);
			isDragging = true;
		}
		
		private void OnRelease(PointerReleaseData releaseData) {
			releasePosition = inputController.ScreenPositionToWorldSpace(releaseData.ReleasePosition);
			pressPosition = releasePosition;
			isDragging = false;
			dragHelper.OnRelease();
		}


		public void OnCellsSelectionChanged() {
			UpdateSelectedElements();
			viewController.ScaleDownUnselectedElements(puzzleElements);
			viewController.ScaleUpSelectedElements(puzzleElements);
		}

		public void OnCellSelectionAccepted() {
			UpdateSelectedElements();

			Link link = new(puzzleElements);
			ExplodeLinkCommand command = new ExplodeLinkCommand(linkManager, link);
			commandInvoker.Enqueue(command);
		}

		private void UpdateSelectedElements() {
			HashList<PuzzleCell> selectedCells = dragHelper.GetSelectedCells();
			puzzleElements.Clear();

			for (int index = 0; index < selectedCells.Count; index++) {
				PuzzleCell selectedCell = selectedCells[index];

				if (!selectedCell.TryGetPuzzleElement(out PuzzleElement puzzleElement))
					break;

				// NOTE This part seems very similar to DragHelper.OnDrag, maybe it can be accessed by LinkManager
				if (puzzleElements.Count == 0)
					puzzleElements.TryAdd(puzzleElement);

				if (puzzleElements[^1].GetDefinition() != puzzleElement.GetDefinition())
					break;

				puzzleElements.TryAdd(puzzleElement);
			}
		}
	}
}
