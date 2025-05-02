using Core.PuzzleElements;
using Core.PuzzleGrids;
using Frolics.Signals;
using UnityEngine;

namespace Core.LinkInput {
	public class PuzzleElementDragHelper {
		private PuzzleGrid puzzleGrid;
		private bool isDraggingMergeItem;

		public PuzzleElementDragHelper() {
			// SceneContext sceneContext = SceneContext.GetInstance();
			// mergeGridContainer = sceneContext.Get<MergeGridContainer>();
			// mergeItemBehaviourFactory = sceneContext.Get<MergeItemBehaviourFactory>();
		}

		public void HandleMergeItemDrag(Vector3 dragPosition) {
			if (!isDraggingMergeItem)
				return;

		}

		public void OnPress(Vector3 pressPosition) {
			if (!puzzleGrid.TryGetPuzzleCell(pressPosition, out PuzzleCell puzzleCell))
				return;

			if (!puzzleCell.TryGetPuzzleElement(out PuzzleElement puzzleElement))
				return;

		
		}

		public void OnRelease(Vector3 releasePosition) {
			if (!isDraggingMergeItem)
				return;

			// MergeGrid mergeGrid = mergeGridContainer.GetMergeGridBehaviour().GetMergeGrid();
			// bool pointerOnCell = mergeGrid.TryGetMergeCell(releasePosition, out MergeCell mergeCell);
			// MergeItem carriedItem = mergeItemBehaviour.GetMergeItem<MergeItem>();
			// isDraggingMergeItem = false;
			//
			// if (pointerOnCell && mergeCell.HasMergeItem()) {
			// 	MergeItem sittingItem = mergeCell.GetMergeItem<MergeItem>();
			// 	ItemDroppedOnItemSignal signal = new(carriedItem, sittingItem, mergeCell);
			// 	SignalBus.GetInstance().Fire(signal);
			// } else if (pointerOnCell) {
			// 	ItemDroppedOnEmptyCellSignal signal = new(carriedItem, mergeCell);
			// 	SignalBus.GetInstance().Fire(signal);
			// } else {
			// 	ItemDroppedOutOfGridSignal signal = new(carriedItem);
			// 	SignalBus.GetInstance().Fire(signal);
			// }
		}
	}
}
