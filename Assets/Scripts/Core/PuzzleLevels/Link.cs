using Core.DataTransfer.Definitions.PuzzleElements;
using Core.PuzzleElements;
using Core.PuzzleGrids;

namespace Core.PuzzleLevels {
	public class Link {
		public const int MinLength = 3;
		
		private readonly HashList<PuzzleElement> puzzleElements;
		private readonly PuzzleElementDefinition elementDefinition;

		public Link(HashList<PuzzleElement> puzzleElements) {
			this.puzzleElements = puzzleElements;
			this.elementDefinition = puzzleElements[0].GetDefinition();
		}

		public Link(PuzzleElement currentElement, PuzzleElement neighborElement) {
			this.puzzleElements = new HashList<PuzzleElement>();
			this.puzzleElements.TryAdd(currentElement);
			this.puzzleElements.TryAdd(neighborElement);

			this.elementDefinition = currentElement.GetDefinition();
		}

		public bool TryAdd(PuzzleElement puzzleElement) {
			return puzzleElements.TryAdd(puzzleElement);
		}

		public bool TryRemove(PuzzleElement puzzleElement) {
			return puzzleElements.TryRemove(puzzleElement);
		}

		public void Explode(PuzzleGrid puzzleGrid) {
			foreach (PuzzleElement puzzleElement in puzzleElements)
				puzzleElement.Explode(puzzleGrid);
		}

		public bool IsValid(PuzzleGrid puzzleGrid) {
			return IsLengthValid() && IsElementsValid() && IsElementsAdjacent(puzzleGrid);
		}

		private bool IsLengthValid() {
			return puzzleElements.Count >= MinLength;
		}

		private bool IsElementsValid() {
			for (int index = 0; index < puzzleElements.Count; index++)
				if (puzzleElements[index].GetDefinition() != elementDefinition)
					return false;

			return true;
		}

		private bool IsElementsAdjacent(PuzzleGrid puzzleGrid) {
			for (int index = 1; index < puzzleElements.Count; index++) {
				PuzzleElement centerElement = puzzleElements[index];
				PuzzleElement adjacentElement = puzzleElements[index - 1];

				if (!puzzleGrid.TryGetPuzzleCell(centerElement, out PuzzleCell centerCell))
					return false;

				if (!puzzleGrid.TryGetPuzzleCell(adjacentElement, out PuzzleCell adjacentCell))
					return false;

				if (!IsCellsAdjacent(puzzleGrid, centerCell, adjacentCell))
					return false;
			}

			return true;
		}

		private bool IsCellsAdjacent(PuzzleGrid puzzleGrid, PuzzleCell centerCell, PuzzleCell adjacentCell) {
			PuzzleCell[] cellNeighbors = puzzleGrid.GetNeighbors(centerCell);

			for (int i = 0; i < cellNeighbors.Length; i++)
				if (adjacentCell == cellNeighbors[i])
					return true;

			return false;
		}

		public HashList<PuzzleElement> GetElements() => puzzleElements;
		public PuzzleElementDefinition GetElementDefinition() => elementDefinition;
	}
}
