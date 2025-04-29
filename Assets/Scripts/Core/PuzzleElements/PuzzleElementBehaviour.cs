using Core.DataTransfer.Definitions;
using Core.PuzzleGrids;
using UnityEngine;

namespace Core.PuzzleElements {
	public abstract class PuzzleElementBehaviour : MonoBehaviour {
		
		public abstract void Initialize(PuzzleElementDefinition definition, PuzzleCell puzzleCell);
		public abstract PuzzleElement GetPuzzleElement();
		public abstract void SetSortingOrder(int sortingOrder);
	}
}
