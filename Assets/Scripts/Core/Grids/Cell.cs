using UnityEngine;

// This could be a MonoBehaviour
namespace Core.Grids {
	public abstract class Cell {
		protected Vector3 worldPosition;
		protected float diameter;

		protected Cell(Vector3 worldPosition, float diameter) {
			this.worldPosition = worldPosition;
			this.diameter = diameter;
		}

		public abstract bool IsInsideCell(Vector3 point);

		public Vector3 GetWorldPosition() => worldPosition;
		public float GetDiameter() => diameter;
	}
}
