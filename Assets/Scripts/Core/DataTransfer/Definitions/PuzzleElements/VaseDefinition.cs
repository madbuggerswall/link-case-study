using Core.PuzzleElements;
using UnityEngine;

namespace Core.DataTransfer.Definitions.PuzzleElements {
	[CreateAssetMenu(fileName = Filename, menuName = MenuName)]

	// NOTE Rename this VaseDefinition
	public class VaseDefinition : PuzzleElementDefinition {
		private const string Filename = nameof(VaseDefinition);
		private const string MenuName = "ScriptableObjects/Definitions/" + Filename;

		[SerializeField] private Sprite sprite;
		[SerializeField] private int durability = 1;

		public override PuzzleElement CreateElement() => new Vase(this);
		
		public override Sprite GetSprite() => sprite;
		public int GetDurability() => durability;
	}
}
