using Core.DataTransfer.Definitions.PuzzleElements;
using Core.PuzzleGrids;
using UnityEngine;

namespace Core.PuzzleElements.Behaviours {
	public abstract class PuzzleElementBehaviour : MonoBehaviour {
		public abstract void Initialize(PuzzleElementDefinition definition, PuzzleCell puzzleCell);

		public abstract void SetSortingOrder(int sortingOrder);
	}
}
