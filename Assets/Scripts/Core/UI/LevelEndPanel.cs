using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Core.UI {
	public class LevelEndPanel : MonoBehaviour {
		[SerializeField] private Button restartButton;

		private void Awake() {
			restartButton.onClick.AddListener(OnRestartButtonClick);
		}

		private void OnRestartButtonClick() {
			Scene activeScene = SceneManager.GetActiveScene();
			SceneManager.LoadScene(activeScene.buildIndex);
		}
	}
}
