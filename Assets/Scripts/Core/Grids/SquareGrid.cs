using Core.Grids.NeighborHelpers;
using Frolics.Utilities;
using UnityEngine;

namespace Core.Grids {
	public abstract class SquareGrid<T> : Grid<T> where T : SquareCell {
		private readonly SquareGridNeighborHelper<T> neighborHelper;

		protected SquareGrid(SquareCellFactory<T> cellFactory, Vector2Int gridSizeInCells, float cellDiameter) {
			Vector2[] cellPositions = GenerateCellPositions(gridSizeInCells);
			
			this.cellDiameter = cellDiameter;
			this.gridSizeInCells = gridSizeInCells;
			this.gridSize = GetFittingGridSize(gridSizeInCells);
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

		private T[] GenerateCells(SquareCellFactory<T> cellFactory, Vector2[] cellPositions) {
			T[] cells = new T[cellPositions.Length];

			for (int i = 0; i < cellPositions.Length; i++)
				cells[i] = cellFactory.Create(cellPositions[i], cellDiameter);

			return cells;
		}

		private Vector2Int GetFittingGridSizeInCells(Vector2 gridSize) {
			int widthInCells = Mathf.CeilToInt(gridSize.x / cellDiameter);
			int heightInCells = Mathf.CeilToInt(gridSize.y / cellDiameter);
			return new Vector2Int(widthInCells, heightInCells);
		}

		private Vector2 GetFittingGridSize(Vector2Int gridSizeInCells) {
			return new Vector2(gridSizeInCells.x * cellDiameter, gridSizeInCells.y * cellDiameter);
		}

		private Vector2[,] GenerateIndexedCellPositions(Vector2Int gridSizeInCells) {
			Vector2[,] cellPositions = new Vector2[gridSizeInCells.x, gridSizeInCells.y];
			Vector2 cellSpacing = new(cellDiameter, cellDiameter);
			Vector2 cellOffset = new(cellDiameter / 2, cellDiameter / 2);
			Vector2Int index = Vector2Int.zero;

			for (index.y = 0; index.y < gridSizeInCells.y; index.y++) {
				for (index.x = 0; index.x < gridSizeInCells.x; index.x++) {
					float cellPosX = cellOffset.x + index.x * cellSpacing.x;
					float cellPosY = cellOffset.y + index.y * cellSpacing.y;
					cellPositions[index.x, index.y] = new Vector2(cellPosX, cellPosY);
				}
			}

			return cellPositions;
		}

		// TODO TryGetNeighbors
		public T[] GetNeighbors(T cell) {
			return neighborHelper.GetCellNeighbors(cell);
		}

		// Grid
		public override T GetCell(Vector2Int index) {
			int clampedX = Mathf.Clamp(index.x, 0, gridSizeInCells.x - 1);
			int clampedY = Mathf.Clamp(index.y, 0, gridSizeInCells.y - 1);
			Vector2Int clampedIndex = new Vector2Int(clampedX, clampedY);

			return cells[clampedIndex.x + clampedIndex.y * gridSizeInCells.x];
		}

		protected override sealed Vector3 CalculateGridCenterPoint(Vector2[] cellPositions) {
			Vector2 positionSum = Vector3.zero;
			for (int i = 0; i < cellPositions.Length; i++)
				positionSum += cellPositions[i];

			return (positionSum / cellPositions.Length).WithZ(0f);
		}
	}

	public abstract class SquareCellFactory<T> where T : SquareCell {
		public abstract T Create(Vector2 cellPosition, float diameter);
	}
}
