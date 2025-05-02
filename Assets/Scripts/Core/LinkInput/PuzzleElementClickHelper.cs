using Core.PuzzleElements;
using Core.PuzzleGrids;
using Frolics.Signals;
using UnityEngine;

namespace Core.LinkInput {
	public class PuzzleElementClickHelper {
		// private readonly MergeGridContainer mergeGridContainer;
		// private readonly MergeItemBehaviourFactory mergeItemBehaviourFactory;

		private PuzzleGrid puzzleGrid;

		public PuzzleElementClickHelper() {
			// SceneContext sceneContext = SceneContext.GetInstance();
			// mergeGridContainer = sceneContext.Get<MergeGridContainer>();
			// mergeItemBehaviourFactory = sceneContext.Get<MergeItemBehaviourFactory>();
		}

		public void OnPress(Vector3 pressPosition) {
			if(!puzzleGrid.TryGetPuzzleCell(pressPosition, out PuzzleCell puzzleCell))
				return;
			if (!puzzleCell.TryGetPuzzleElement(out PuzzleElement puzzleElement))
				return;
			
		}

		public void OnRelease(Vector3 releasePosition) {
			if(!puzzleGrid.TryGetPuzzleCell(releasePosition, out PuzzleCell puzzleCell))
				return;

			if (!puzzleCell.TryGetPuzzleElement(out PuzzleElement puzzleElement))
				return;


			// if (pointerOnCell && !puzzleCell.IsEmpty()) {
			// 	MergeItem clickedItem = mergeItemBehaviour.GetMergeItem<MergeItem>();
			// 	ClickedOnItemSignal signal = new(clickedItem);
			// 	SignalBus.GetInstance().Fire(signal);
			// } else if (pointerOnCell) {
			// 	ClickedOnEmptyCellSignal signal = new(mergeCell);
			// 	SignalBus.GetInstance().Fire(signal);
			// } else {
			// 	ClickedOutOfGridSignal signal = new();
			// 	SignalBus.GetInstance().Fire(signal);
			// }
		}
	}
}
