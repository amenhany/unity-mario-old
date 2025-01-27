using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
	public Animator animator;
	public Rigidbody2D rb;
	public GameObject marioLives;

	void Awake() {
		marioLives = GameObject.Find("MarioLives");
	}

	void Start() {
		StartCoroutine(DeathAnimation());
	}

	IEnumerator DeathAnimation() {
		yield return new WaitForSeconds(0.85f);
		animator.enabled = true;
		rb.bodyType = RigidbodyType2D.Dynamic;
		rb.velocity = new Vector2(0f, 22f);

		yield return new WaitForSeconds(3);
		marioLives.GetComponent<Counter>().Add(-1);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
