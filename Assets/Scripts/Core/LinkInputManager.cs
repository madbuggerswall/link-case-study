using Core.Contexts;
using Core.PuzzleElements;
using Core.PuzzleGrids;
using Frolics.Tween;
using UnityEngine;

namespace Core {
	public class LinkInputManager : MonoBehaviour {
		private readonly HashList<PuzzleElement> puzzleElements = new();

		private PuzzleCellDragHelper dragHelper;
		private PuzzleElementBehaviourFactory elementBehaviourFactory;
		private PuzzleLevelInitializer puzzleLevelInitializer;

		public void Initialize() {
			SceneContext sceneContext = SceneContext.GetInstance();
			dragHelper = sceneContext.Get<PuzzleCellDragHelper>();
			elementBehaviourFactory = sceneContext.Get<PuzzleElementBehaviourFactory>();
			puzzleLevelInitializer = sceneContext.Get<PuzzleLevelInitializer>();

			dragHelper.OnCellsChanged.AddListener(OnCellsChanged);
			dragHelper.OnCellsSelected.AddListener(OnCellsSelected);
		}

		private void OnCellsChanged() {
			EvaluateSelectedCells();
			
			PuzzleCell[] puzzleCells = puzzleLevelInitializer.PuzzleGrid.GetCells();
			for (int index = 0; index < puzzleCells.Length; index++) {
				PuzzleCell puzzleCell = puzzleCells[index];
				if(!puzzleCell.TryGetPuzzleElement(out PuzzleElement puzzleElement))
					return;

				PuzzleElementBehaviour elementBehaviour = elementBehaviourFactory.GetPuzzleElementBehaviour(puzzleElement);
				elementBehaviour.transform.localScale = Vector3.one;
			}

			for (int index = 0; index < puzzleElements.Count; index++) {
				PuzzleElement puzzleElement = puzzleElements[index];
				PuzzleElementBehaviour elementBehaviour = elementBehaviourFactory.GetPuzzleElementBehaviour(puzzleElement);

				TransformTween tween = new TransformTween(elementBehaviour.transform, 1f);
				tween.SetLocalScale(Vector3.one * 1.2f);
				tween.Play();
			}
		}

		private void OnCellsSelected() {
	
		}

		private void EvaluateSelectedCells() {
			HashList<PuzzleCell> selectedCells = dragHelper.PuzzleCells;
			puzzleElements.Clear();

			for (int index = 0; index < selectedCells.Count; index++) {
				PuzzleCell selectedCell = selectedCells[index];

				if (!selectedCell.TryGetPuzzleElement(out PuzzleElement puzzleElement))
					break;

				if (puzzleElements.Count == 0)
					puzzleElements.TryAdd(puzzleElement);

				if (puzzleElements[^1].GetDefinition() != puzzleElement.GetDefinition())
					break;

				puzzleElements.TryAdd(puzzleElement);
			}
		}
	}
}
