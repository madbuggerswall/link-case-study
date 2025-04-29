using Core.Grids.NeighborHelpers;
using Frolics.Utilities;
using UnityEngine;

namespace Core.Grids {
	public class CircleGrid<T> : Grid<T> where T : CircleCell {
		private readonly CircleGridNeighborHelper<T> neighborHelper;

		public CircleGrid(CellFactory<T> cellFactory, Vector2Int gridSizeInCells, float cellDiameter) {
			this.cellDiameter = cellDiameter;
			this.gridSizeInCells = gridSizeInCells;
			this.gridSize = GetFittingGridSize(gridSizeInCells);

			Vector2[] cellPositions = GenerateCellPositions(gridSizeInCells);
			this.centerPoint = CalculateGridCenterPoint(cellPositions);
			this.cells = GenerateCells(cellFactory, cellPositions);

			this.neighborHelper = new CircleGridNeighborHelper<T>(this);
		}

		private Vector2 GetFittingGridSize(Vector2Int gridSizeInCells) {
			float sizeX = gridSizeInCells.x * cellDiameter;
			float sizeY = (gridSizeInCells.y - 1) * cellDiameter * Mathf.Cos(30 * Mathf.Deg2Rad);
			return new Vector2(sizeX, sizeY);
		}

		private Vector2[] GenerateCellPositions(Vector2Int gridSizeInCells) {
			int evenRowCount = Mathf.CeilToInt(gridSizeInCells.y / 2f);
			Vector2[] cellPositions = new Vector2[gridSizeInCells.x * gridSizeInCells.y + evenRowCount];
			Vector2 cellSpacing = new(cellDiameter, cellDiameter * Mathf.Cos(30 * Mathf.Deg2Rad));

			Vector2Int index = Vector2Int.zero;

			for (index.y = 0; index.y < gridSizeInCells.y; index.y++) {
				Vector2 cellOffset = new(index.y % 2 == 0 ? 0 : (cellDiameter / 2), 0f);
				int rowSizeInCells = index.y % 2 == 0 ? (gridSizeInCells.x + 1) : gridSizeInCells.x;

				for (index.x = 0; index.x < rowSizeInCells; index.x++) {
					float cellPosX = cellOffset.x + index.x * cellSpacing.x;
					float cellPosY = cellOffset.y + index.y * cellSpacing.y;

					int evenRowsPassed = Mathf.CeilToInt(index.y / 2f);
					int positionIndex = index.x + index.y * gridSizeInCells.x + evenRowsPassed;
					cellPositions[positionIndex] = new Vector2(cellPosX, cellPosY);
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

		public T[] GetNeighbors(T cell) {
			return neighborHelper.GetCellNeighbors(cell);
		}
	}
}
