using Core.Grids.NeighborHelpers;
using Frolics.Utilities;
using UnityEngine;

namespace Core.Grids {
	public abstract class SquareGrid<T> : Grid<T> where T : SquareCell {
		protected readonly SquareGridNeighborHelper<T> neighborHelper;

		protected SquareGrid(CellFactory<T> cellFactory, Vector2Int gridSizeInCells, float cellDiameter) {
			this.cellDiameter = cellDiameter;
			this.gridSizeInCells = gridSizeInCells;
			this.gridSize = GetFittingGridSize(gridSizeInCells);

			Vector2[] cellPositions = GenerateCellPositions(gridSizeInCells);
			this.centerPoint = CalculateGridCenterPoint(cellPositions);
			this.cells = GenerateCells(cellFactory, cellPositions);

			this.neighborHelper = new SquareGridNeighborHelper<T>(this, false);
		}

		private Vector2[] GenerateCellPositions(Vector2Int gridSizeInCells) {
			Vector2[] cellPositions = new Vector2[gridSizeInCells.x * gridSizeInCells.y];
			Vector2 cellSpacing = new(cellDiameter, cellDiameter);
			Vector2 cellOffset = new(cellDiameter / 2, cellDiameter / 2);
			Vector2Int index = Vector2Int.zero;

			for (index.y = 0; index.y < gridSizeInCells.y; index.y++) {
				for (index.x = 0; index.x < gridSizeInCells.x; index.x++) {
					float cellPosX = cellOffset.x + index.x * cellSpacing.x;
					float cellPosY = cellOffset.y + index.y * cellSpacing.y;
					cellPositions[index.x + index.y * gridSizeInCells.x] = new Vector2(cellPosX, cellPosY);
				}
			}

			return cellPositions;
		}

		private Vector3 CalculateGridCenterPoint(Vector2[] cellPositions) {
			Vector2 positionSum = Vector3.zero;
			for (int i = 0; i < cellPositions.Length; i++)
				positionSum += cellPositions[i];

			return (positionSum / cellPositions.Length).WithZ(0f);
		}

		private Vector2 GetFittingGridSize(Vector2Int gridSizeInCells) {
			return new Vector2(gridSizeInCells.x * cellDiameter, gridSizeInCells.y * cellDiameter);
		}

		public T[] GetNeighbors(T cell) {
			return neighborHelper.GetCellNeighbors(cell);
		}
	}
}
