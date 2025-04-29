using Core.PuzzleElements;
using UnityEngine;

namespace Core.Grids {
	public abstract class Grid<T> where T : Cell {
		protected T[] cells;

		protected Vector2 gridSize;
		protected Vector2Int gridSizeInCells;
		protected Vector3 centerPoint;
		protected float cellDiameter;

		// NOTE Possibly redundant
		public abstract T GetCell(Vector2Int index);
		protected abstract Vector3 CalculateGridCenterPoint(Vector2[] cellPositions);

		
		public Vector2 GetGridSize() => gridSize;
		public Vector2Int GetGridSizeInCells() => gridSizeInCells;

		public float GetCellDiameter() => cellDiameter;
		public Vector3 GetCenterPoint() => centerPoint;

		public T GetCell(int index) => cells[index];
		public T[] GetCells() => cells;
	}
}
