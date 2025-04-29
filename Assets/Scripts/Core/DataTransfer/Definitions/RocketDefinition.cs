using Core.PuzzleElements;
using UnityEngine;

namespace Core.DataTransfer.Definitions {
	[CreateAssetMenu(fileName = Filename, menuName = MenuName)]
	public class RocketDefinition : PuzzleElementDefinition {
		private const string Filename = nameof(RocketDefinition);
		private const string MenuName = "ScriptableObjects/Definitions/" + Filename;

		[SerializeField] private Sprite leftRocketSprite;
		[SerializeField] private Sprite rightRocketSprite;

		public Sprite GetLeftRocketSprite() => leftRocketSprite;
		public Sprite GetRightRocketSprite() => rightRocketSprite;
		public override PuzzleElement CreateElement() => new Rocket(this);
	}
}
