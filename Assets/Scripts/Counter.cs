using TMPro;
using UnityEngine;

public class Counter : MonoBehaviour
{
	[SerializeField] bool coins;
	AudioManager audioManager;
	GameObject marioLives;

	int counter = 0;
	[SerializeField] string stringType = "0";
	[SerializeField] TextMeshProUGUI counterText;

	private void Awake() {
		marioLives = GameObject.Find("MarioLives");
	}

	public void Add(int amount) {
		audioManager = FindObjectOfType<AudioManager>();
		counter += amount;

		if (counter < 0) {
			counter = 0;
		}

		if (counter > 99) {
			if (coins) {
				marioLives.GetComponent<Counter>().Add(1);
				audioManager.Play("1-Up");
				counter -= 100;
			} else counter = 99;
		}

		counterText.text = counter.ToString(stringType);
	}
}
