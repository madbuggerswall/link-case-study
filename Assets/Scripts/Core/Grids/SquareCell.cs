using UnityEngine;

namespace Core.Grids {
	public class SquareCell : Cell {
		public SquareCell(Vector3 worldPosition, float diameter) : base(worldPosition, diameter) {
			this.worldPosition = worldPosition;
			this.diameter = diameter;
		}

		public override bool IsInsideCell(Vector3 point) {
			float radius = diameter / 2f;
			bool inHorizontally = worldPosition.x - radius <= point.x && point.x <= worldPosition.x + radius;
			bool inVertically = worldPosition.y - radius <= point.y && point.y <= worldPosition.y + radius;

			return inHorizontally && inVertically;
		}
	}
}
