using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Grids.NeighborHelpers {
	public class SquareGridNeighborHelper<T> where T : SquareCell {
		private readonly Dictionary<T, T[]> neighborsByCell;
		private readonly int gridWidth;
		private readonly int gridHeight;
		private readonly bool edgesOnly;

		public SquareGridNeighborHelper(SquareGrid<T> squareGrid, bool edgesOnly = false) {
			this.gridWidth = squareGrid.GetGridSizeInCells().x;
			this.gridHeight = squareGrid.GetGridSizeInCells().y;
			this.edgesOnly = edgesOnly;

			this.neighborsByCell = MapNeighborsByCell(squareGrid.GetCells());
		}

		public T[] GetCellNeighbors(T cell) {
			return neighborsByCell.TryGetValue(cell, out T[] neighbors) ? neighbors : new T[] { };
		}

		private Dictionary<T, T[]> MapNeighborsByCell(T[] cells) {
			Dictionary<T, T[]> neighborsByCell = new();
			for (int i = 0; i < cells.Length; i++)
				neighborsByCell.Add(cells[i], GetCellNeighbors(cells, i));

			return neighborsByCell;
		}

		private T[] GetCellNeighbors(T[] cells, int cellIndex) {
			Position position = GetPosition(cellIndex);

			return position switch {
				Position.BottomLeft => GetSelectedCellNeighbors(cells, cellIndex, 0, 1, 2),
				Position.BottomRight => GetSelectedCellNeighbors(cells, cellIndex, 2, 3, 4),
				Position.TopLeft => GetSelectedCellNeighbors(cells, cellIndex, 0, 6, 7),
				Position.TopRight => GetSelectedCellNeighbors(cells, cellIndex, 4, 5, 6),
				Position.Bottom => GetSelectedCellNeighbors(cells, cellIndex, 0, 1, 2, 3, 4),
				Position.Top => GetSelectedCellNeighbors(cells, cellIndex, 4, 5, 6, 7, 0),
				Position.Left => GetSelectedCellNeighbors(cells, cellIndex, 0, 1, 2, 6, 7),
				Position.Right => GetSelectedCellNeighbors(cells, cellIndex, 2, 3, 4, 5, 6),
				Position.Center => GetSelectedCellNeighbors(cells, cellIndex, 0, 1, 2, 3, 4, 5, 6, 7),
				_ => GetSelectedCellNeighbors(cells, cellIndex, 0, 1, 2, 3, 4, 5, 6, 7),
			};
		}

		private T[] GetSelectedCellNeighbors(T[] cells, int cellIndex, params int[] selectedIndices) {
			int capacity = edgesOnly ? GetEdgeNeighborIndexCount(selectedIndices) : selectedIndices.Length;
			T[] selectedCellNeighbors = new T[capacity];

			for (int i = 0, j = 0; i < selectedIndices.Length && j < capacity; i++) {
				if (edgesOnly && selectedIndices[i] % 2 != 0)
					continue;

				int selectedNeighborIndex = GetNeighborIndex(selectedIndices[i], cellIndex);
				selectedCellNeighbors[j++] = cells[selectedNeighborIndex];
			}

			return selectedCellNeighbors;
		}

		private int GetEdgeNeighborIndexCount(int[] selectedIndices) {
			int edgeNeighborIndexCount = 0;

			for (int i = 0; i < selectedIndices.Length; i++)
				if (selectedIndices[i] % 2 == 0)
					edgeNeighborIndexCount++;

			return edgeNeighborIndexCount;
		}

		private int GetNeighborIndex(int neighborIndex, int cellIndex) {
			return neighborIndex switch {
				0 => cellIndex + 1,
				1 => cellIndex + gridWidth + 1,
				2 => cellIndex + gridWidth,
				3 => cellIndex + gridWidth - 1,
				4 => cellIndex - 1,
				5 => cellIndex - gridWidth - 1,
				6 => cellIndex - gridWidth,
				7 => cellIndex - gridWidth + 1,
				_ => throw new ArgumentOutOfRangeException(nameof(neighborIndex), neighborIndex, null)
			};
		}

		// Grid relative position utilities
		private Position GetPosition(int cellIndex) {
			if (IsBottomLeftCorner(cellIndex))
				return Position.BottomLeft;
			else if (IsBottomRightCorner(cellIndex))
				return Position.BottomRight;
			else if (IsTopLeftCorner(cellIndex))
				return Position.TopLeft;
			else if (IsTopRightCorner(cellIndex))
				return Position.TopRight;

			else if (IsBottomEdge(cellIndex))
				return Position.Bottom;
			else if (IsTopEdge(cellIndex))
				return Position.Top;
			else if (IsLeftEdge(cellIndex))
				return Position.Left;
			else if (IsRightEdge(cellIndex))
				return Position.Right;

			else
				return Position.Center;
		}

		private int GetTopRightCorner() => gridWidth * gridHeight - 1;
		private int GetTopLeftCorner() => gridWidth * (gridHeight - 1);
		private int GetBottomRightCorner() => gridWidth - 1;
		private int GetBottomLeftCorner() => 0;

		private bool IsTopRightCorner(int cellIndex) => cellIndex == GetTopRightCorner();
		private bool IsTopLeftCorner(int cellIndex) => cellIndex == GetTopLeftCorner();
		private bool IsBottomRightCorner(int cellIndex) => cellIndex == GetBottomRightCorner();
		private bool IsBottomLeftCorner(int cellIndex) => cellIndex == GetBottomLeftCorner();

		private bool IsTopEdge(int cellIndex) => InRange(cellIndex, GetTopLeftCorner(), GetTopRightCorner());
		private bool IsBottomEdge(int cellIndex) => InRange(cellIndex, GetBottomLeftCorner(), GetBottomRightCorner());
		private bool IsRightEdge(int cellIndex) => (cellIndex - (gridWidth - 1)) % gridWidth == 0;
		private bool IsLeftEdge(int cellIndex) => cellIndex % gridWidth == 0;

		private enum Position { TopLeft, TopRight, BottomLeft, BottomRight, Top, Bottom, Left, Right, Center }

		private bool InRange(int value, int min, int max) => value > min && value < max;
	}
}
