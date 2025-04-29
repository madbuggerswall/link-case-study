using UnityEngine;

namespace Core.Grids {
	public abstract class Grid<T> where T : Cell {
		protected T[] cells;

		protected Vector2 gridSize;
		protected Vector2Int gridSizeInCells;
		protected Vector3 centerPoint;
		protected float cellDiameter;

		protected T[] GenerateCells(CellFactory<T> cellFactory, Vector2[] cellPositions) {
			T[] cells = new T[cellPositions.Length];

			for (int i = 0; i < cellPositions.Length; i++)
				cells[i] = cellFactory.Create(cellPositions[i], cellDiameter);

			return cells;
		}

		// Getters
		public Vector2 GetGridSize() => gridSize;
		public Vector2Int GetGridSizeInCells() => gridSizeInCells;

		public float GetCellDiameter() => cellDiameter;
		public Vector3 GetCenterPoint() => centerPoint;

		public T GetCell(int index) => cells[index];
		public T[] GetCells() => cells;
	}
}
