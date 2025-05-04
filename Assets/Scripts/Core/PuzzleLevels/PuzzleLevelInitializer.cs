using Core.Contexts;
using Core.DataTransfer.Definitions;
using Core.PuzzleElements;
using Core.PuzzleGrids;
using UnityEngine;

namespace Core.PuzzleLevels {
	// NOTE Rename this to PuzzleLevelLoader
	public class PuzzleLevelInitializer : MonoBehaviour,IInitializable {
		[SerializeField] private PuzzleLevelDefinition levelDefinition;
		
		// Dependencies
		private ChipDefinitionManager chipDefinitionManager;
		private CameraController cameraController;

		// Fields
		private PuzzleGrid puzzleGrid;

		// TODO Initialize Targets
		// TODO Initialize TurnCount

		public void Initialize() {
			chipDefinitionManager = SceneContext.GetInstance().Get<ChipDefinitionManager>();
			cameraController = SceneContext.GetInstance().Get<CameraController>();

			// Puzzle Grid
			InitializeGrid();
			InitializeElements();
			InitializePlacedElements();

			// Camera Controller
			cameraController.CenterCameraToGrid(puzzleGrid);
			cameraController.AdjustOrthographicSizeToFit(puzzleGrid);
		}

		private void InitializeGrid() {
			const float cellDiameter = 1.2f;

			Vector2Int gridSize = levelDefinition.GetGridSize();
			puzzleGrid = new PuzzleGrid(gridSize, cellDiameter);
		}

		private void InitializeElements() {
			PuzzleCell[] puzzleCells = puzzleGrid.GetCells();

			for (int index = 0; index < puzzleCells.Length; index++) {
				PuzzleCell puzzleCell = puzzleCells[index];
				PuzzleElement colorChip = CreateRandomColorChip();
				puzzleCell.SetPuzzleElement(colorChip);
			}
		}

		private void InitializePlacedElements() {
			ElementPlacementDTO[] elementPlacements = levelDefinition.GetElementPlacements();
			PuzzleCell[] puzzleCells = puzzleGrid.GetCells();

			for (int i = 0; i < elementPlacements.Length; i++) {
				int cellIndex = elementPlacements[i].GetPositionIndex();
				PuzzleElementDefinition definition = elementPlacements[i].GetPuzzleElementDefinition();
				PuzzleElement puzzleElement = definition.CreateElement();

				PuzzleCell puzzleCell = puzzleCells[cellIndex];
				puzzleCell.SetPuzzleElement(puzzleElement);
			}
		}

		private PuzzleElement CreateRandomColorChip() {
			ColorChipDefinition colorChipDefinition = chipDefinitionManager.GetRandomColorChipDefinition();
			ColorChip colorChip = new ColorChip(colorChipDefinition);

			return colorChip;
		}
		
		public PuzzleGrid GetPuzzleGrid() => puzzleGrid;
	}
}
