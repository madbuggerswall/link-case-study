using Core.DataTransfer.Definitions;

namespace Core.PuzzleElements {
	public abstract class PuzzleElement {
		private readonly PuzzleElementDefinition definition;

		protected PuzzleElement(PuzzleElementDefinition definition) {
			this.definition = definition;
		}

		protected abstract void Explode();
		protected abstract void OnAdjacentExplode();

		public PuzzleElementDefinition GetDefinition() => definition;
	}

	public class ColorChip : PuzzleElement {
		public ColorChip(ColorChipDefinition definition) : base(definition) { }

		protected override void Explode() {
			throw new System.NotImplementedException();
		}

		protected override void OnAdjacentExplode() {
			throw new System.NotImplementedException();
		}
	}

	public class Balloon : PuzzleElement {
		public Balloon(BalloonDefinition definition) : base(definition) { }

		protected override void Explode() {
			throw new System.NotImplementedException();
		}

		protected override void OnAdjacentExplode() {
			throw new System.NotImplementedException();
		}
	}

	public class Duck : PuzzleElement {
		public Duck(DuckDefinition definition) : base(definition) { }

		protected override void Explode() {
			throw new System.NotImplementedException();
		}

		protected override void OnAdjacentExplode() {
			throw new System.NotImplementedException();
		}
	}

	public class Rocket : PuzzleElement {
		public Rocket(RocketDefinition definition) : base(definition) { }

		protected override void Explode() {
			throw new System.NotImplementedException();
		}

		protected override void OnAdjacentExplode() {
			throw new System.NotImplementedException();
		}
	}
}
