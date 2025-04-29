using UnityEngine;

namespace Frolics.Tween {
	public class ColorTween : Tween {
		private readonly SpriteRenderer tweener;
		private readonly Color initialColor;
		private Color targetColor;

		public ColorTween(SpriteRenderer tweener, Color targetColor, float duration) : base(duration) {
			this.tweener = tweener;
			this.initialColor = tweener.color;
			this.targetColor = targetColor;
		}

		protected override void UpdateTween() {
			tweener.color = Color.Lerp(initialColor, targetColor, progress);
		}
	}
}
