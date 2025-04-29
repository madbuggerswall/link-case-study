using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Grids.NeighborHelpers {
	public class CircleGridNeighborHelper<T> where T : CircleCell {
		private readonly int gridWidth;
		private readonly int gridHeight;
		private readonly Dictionary<T, T[]> neighborsByCell;


		public CircleGridNeighborHelper(CircleGrid<T> circleGrid) {
			this.gridWidth = circleGrid.GetGridSizeInCells().x;
			this.gridHeight = circleGrid.GetGridSizeInCells().y;

			this.neighborsByCell = MapNeighborsByCell(circleGrid.GetCells());
		}

		public T[] GetCellNeighbors(T cell) {
			if (neighborsByCell.TryGetValue(cell, out T[] neighbors))
				return neighbors;

			return new T[] { };
		}

		private Dictionary<T, T[]> MapNeighborsByCell(T[] circleCells) {
			Dictionary<T, T[]> neighborsByCell = new();

			for (int i = 0; i < circleCells.Length; i++)
				neighborsByCell.Add(circleCells[i], GetCellNeighbors(circleCells, i));

			return neighborsByCell;
		}

		private T[] GetCellNeighbors(T[] cells, int cellIndex) {
			Position position = GetCellPosition(cellIndex);

			return position switch {
				Position.BottomLeftCorner => GetSelectedCellNeighbors(cells, cellIndex, 0, 1),
				Position.BottomRightCorner => GetSelectedCellNeighbors(cells, cellIndex, 2, 3),
				Position.TopLeftCorner => GetSelectedCellNeighbors(cells, cellIndex, 5, 0),
				Position.TopRightCorner => GetSelectedCellNeighbors(cells, cellIndex, 3, 4),

				Position.BottomEdge => GetSelectedCellNeighbors(cells, cellIndex, 0, 1, 2, 3),
				Position.TopEdge => GetSelectedCellNeighbors(cells, cellIndex, 0, 3, 4, 5),

				Position.LeftOuterEdge => GetSelectedCellNeighbors(cells, cellIndex, 0, 1, 5),
				Position.LeftInnerEdge => GetSelectedCellNeighbors(cells, cellIndex, 0, 1, 2, 4, 5),
				Position.RightOuterEdge => GetSelectedCellNeighbors(cells, cellIndex, 2, 3, 4),
				Position.RightInnerEdge => GetSelectedCellNeighbors(cells, cellIndex, 1, 2, 3, 4, 5),

				Position.Center => GetSelectedCellNeighbors(cells, cellIndex, 0, 1, 2, 3, 4, 5),
				_ => throw new ArgumentOutOfRangeException()
			};
		}

		private Position GetCellPosition(int cellIndex) {
			Position position = Position.Center;

			position = IsBottomLeftCorner(cellIndex) ? Position.BottomLeftCorner : position;
			position = IsBottomRightCorner(cellIndex) ? Position.BottomRightCorner : position;
			position = IsTopLeftCorner(cellIndex) ? Position.TopLeftCorner : position;
			position = IsTopRightCorner(cellIndex) ? Position.TopRightCorner : position;

			position = IsBottomEdge(cellIndex) ? Position.BottomEdge : position;
			position = IsTopEdge(cellIndex) ? Position.TopEdge : position;

			position = IsLeftOuterEdge(cellIndex) ? Position.LeftOuterEdge : position;
			position = IsLeftInnerEdge(cellIndex) ? Position.LeftInnerEdge : position;
			position = IsRightOuterEdge(cellIndex) ? Position.RightOuterEdge : position;
			position = IsRightInnerEdge(cellIndex) ? Position.RightInnerEdge : position;

			return position;
		}

		private T[] GetSelectedCellNeighbors(T[] cells, int cellIndex, params int[] selectedIndices) {
			T[] selectedCellNeighbors = new T[selectedIndices.Length];

			for (int i = 0; i < selectedIndices.Length; i++) {
				int neighborCellIndex = GetNeighborCellIndex(cellIndex, selectedIndices[i]);
				selectedCellNeighbors[i] = cells[neighborCellIndex];
			}

			return selectedCellNeighbors;
		}

		private int GetNeighborCellIndex(int neighborIndex, int cellIndex) {
			return neighborIndex switch {
				0 => cellIndex + 1,
				1 => cellIndex + gridWidth + 1,
				2 => cellIndex + gridWidth,
				3 => cellIndex - 1,
				4 => cellIndex - gridWidth - 1,
				5 => cellIndex - gridWidth,
				_ => throw new ArgumentOutOfRangeException(nameof(neighborIndex), neighborIndex, null)
			};
		}


		private int GetTopRightCorner() => gridWidth * gridHeight + GetEvenRowCount(gridHeight) - 1;
		private int GetTopLeftCorner() => gridWidth * (gridHeight - 1) + GetEvenRowCount(gridHeight - 1);
		private int GetBottomRightCorner() => gridWidth;
		private int GetBottomLeftCorner() => 0;

		private bool IsTopRightCorner(int cellIndex) => cellIndex == GetTopRightCorner();
		private bool IsTopLeftCorner(int cellIndex) => cellIndex == GetTopLeftCorner();
		private bool IsBottomRightCorner(int cellIndex) => cellIndex == GetBottomRightCorner();
		private bool IsBottomLeftCorner(int cellIndex) => cellIndex == GetBottomLeftCorner();
		private bool IsTopEdge(int cellIndex) => InRange(cellIndex, GetTopLeftCorner(), GetTopRightCorner());
		private bool IsBottomEdge(int cellIndex) => InRange(cellIndex, GetBottomLeftCorner(), GetBottomRightCorner());
		private bool IsRightInnerEdge(int cellIndex) => (cellIndex - 2 * gridWidth) % (2 * gridWidth + 1) == 0;
		private bool IsRightOuterEdge(int cellIndex) => (cellIndex - gridWidth) % (2 * gridWidth + 1) == 0;
		private bool IsLeftInnerEdge(int cellIndex) => (cellIndex - (gridWidth + 1)) % (2 * gridWidth + 1) == 0;
		private bool IsLeftOuterEdge(int cellIndex) => cellIndex % (2 * gridWidth + 1) == 0;

		private bool InRange(int value, int min, int max) => value > min && value < max;
		private int GetEvenRowCount(int height) => Mathf.CeilToInt(height / 2f);

		private enum Position {
			BottomLeftCorner,
			BottomRightCorner,
			TopLeftCorner,
			TopRightCorner,
			BottomEdge,
			TopEdge,
			LeftOuterEdge,
			LeftInnerEdge,
			RightOuterEdge,
			RightInnerEdge,
			Center
		}
	}
}
