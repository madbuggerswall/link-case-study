using Core.DataTransfer.Definitions;
using Core.PuzzleElements;

namespace Core.Links {
	public class Link {
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

		public bool IsValid() {
			return IsLengthValid() && IsElementsValid();
		}

		private bool IsLengthValid() {
			const int minLength = 3;
			return puzzleElements.Count >= minLength;
		}

		private bool IsElementsValid() {
			for (int index = 0; index < puzzleElements.Count; index++)
				if (puzzleElements[index].GetDefinition() != elementDefinition)
					return false;

			return true;
		}

		// NOTE Explode should be called via LinkManager.Explode(Link)
		public void Explode() {
			for (int index = 0; index < puzzleElements.Count; index++) {
				PuzzleElement puzzleElement = puzzleElements[index];
				puzzleElement.Explode();
			}
		}
	}
}
