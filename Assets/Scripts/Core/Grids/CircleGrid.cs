using Core.Grids.NeighborHelpers;
using Frolics.Utilities;
using UnityEngine;

namespace Core.Grids {
	public class CircleGrid<T> : Grid<T> where T : CircleCell {
		private readonly CircleGridNeighborHelper<T> neighborHelper;

		public CircleGrid(CircleCellFactory<T> cellFactory, Vector2Int gridSizeInCells, float cellDiameter) {
			this.gridSizeInCells = gridSizeInCells;
			this.cellDiameter = cellDiameter;
			this.gridSize = GetFittingGridSize(gridSizeInCells);

			Vector2[] cellPositions = GenerateCellPositions(gridSizeInCells);
			this.centerPoint = CalculateGridCenterPoint(cellPositions);
			this.cells = GenerateCells(cellFactory, cellPositions);

			this.neighborHelper = new CircleGridNeighborHelper<T>(this);
		}
		
		private T[] GenerateCells(CircleCellFactory<T> cellFactory, Vector2[] cellPositions) {
			T[] cells = new T[cellPositions.Length];

			for (int i = 0; i < cellPositions.Length; i++)
				cells[i] = cellFactory.CreateCell(cellPositions[i], cellDiameter);

			return cells;
		}


		private Vector2Int GetFittingGridSizeInCells(Vector2 gridSize) {
			int widthInCells = Mathf.CeilToInt(gridSize.x / cellDiameter);
			int heightInCells = Mathf.CeilToInt(gridSize.y / cellDiameter);
			return new Vector2Int(widthInCells, heightInCells);
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

		private Vector2[,] GenerateIndexedCellPositions(Vector2Int gridSizeInCells) {
			Vector2[,] cellPositions = new Vector2[gridSizeInCells.x + 1, gridSizeInCells.y];
			Vector2 cellSpacing = new(cellDiameter, cellDiameter * Mathf.Cos(30));

			Vector2Int index = Vector2Int.zero;

			for (index.y = 0; index.y < gridSizeInCells.y; index.y++) {
				Vector2 cellOffset = new(index.y % 2 == 0 ? 0 : (cellDiameter / 2), 0f);
				int rowSizeInCells = index.y % 2 == 0 ? gridSizeInCells.x : (gridSizeInCells.x - 1);

				for (index.x = 0; index.x < rowSizeInCells; index.x++) {
					float cellPosX = cellOffset.x + index.x * cellSpacing.x;
					float cellPosY = cellOffset.y + index.y * cellSpacing.y;
					cellPositions[index.x, index.y] = new Vector2(cellPosX, cellPosY);
				}
			}

			return cellPositions;
		}


		// Grid
		protected override sealed Vector3 CalculateGridCenterPoint(Vector2[] cellPositions) {
			Vector2 positionSum = Vector3.zero;
			for (int i = 0; i < cellPositions.Length; i++)
				positionSum += cellPositions[i];

			return (positionSum / cellPositions.Length).WithZ(0f);
		}

		public override T GetCell(Vector2Int index) {
			bool isEvenRow = index.y % 2 == 0;
			int clampedX = Mathf.Clamp(index.x, 0, gridSizeInCells.x - 1);
			int clampedY = Mathf.Clamp(index.y, 0, gridSizeInCells.y - (isEvenRow ? 1 : 2));

			// Odd rows has 1 cell less than even rows, because of centering strategy
			Vector2Int clampedIndex = new Vector2Int(clampedX, clampedY);
			int oddRowCount = Mathf.FloorToInt(index.y / 2f);

			return cells[clampedIndex.x + clampedIndex.y * gridSizeInCells.x - oddRowCount];
		}
	}

	public abstract class CircleCellFactory<T> where T : CircleCell {
		public abstract T CreateCell(Vector2 cellPosition, float diameter);
	}
}
