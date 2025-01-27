using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
	public static LevelManager instance;

	private void Awake() {
		instance = this;
	}

	void Update() {
		if (Input.GetButtonDown("Reset")) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
