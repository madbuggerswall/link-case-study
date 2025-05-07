using Core.Contexts;
using UnityEngine;

namespace Core.DataTransfer.Definitions.PuzzleElements {
	public class ChipDefinitionManager : MonoBehaviour, IInitializable {
		[Header("Color Chip Definitions")]
		[SerializeField] private ColorChipDefinition[] colorChipDefinitions;

		public void Initialize() { }

		public ColorChipDefinition GetRandomColorChipDefinition() {
			int randomIndex = Random.Range(0, colorChipDefinitions.Length);
			ColorChipDefinition colorChipDefinition = colorChipDefinitions[randomIndex];
			return colorChipDefinition;
		}
	}
}
