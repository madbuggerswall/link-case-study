using Core.PuzzleElements;
using UnityEngine;

namespace Core.DataTransfer.Definitions {
	[CreateAssetMenu(fileName = Filename, menuName = MenuName)]
	public class RocketDefinition : PuzzleElementDefinition {
		private const string Filename = nameof(RocketDefinition);
		private const string MenuName = "ScriptableObjects/Definitions/" + Filename;

		[SerializeField] private ColorChipDefinition colorChipDefinition;
		[SerializeField] private Sprite leftRocketSprite;
		[SerializeField] private Sprite rightRocketSprite;

		public override PuzzleElement CreateElement() => new Rocket(this);
		
		public ColorChipDefinition GetColorChipDefinition() => colorChipDefinition;
		public Sprite GetLeftRocketSprite() => leftRocketSprite;
		public Sprite GetRightRocketSprite() => rightRocketSprite;
	}
}
