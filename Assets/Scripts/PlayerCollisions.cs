using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollisions : MonoBehaviour
{
	[SerializeField] private PlayerMovement playerMovement;
	[SerializeField] private Animator animator;
	[SerializeField] private SpriteRenderer spriteRenderer;
	[SerializeField] private Collider2D standCollider;
	[SerializeField] private Collider2D crouchCollider;
	[SerializeField] private Rigidbody2D rb;
	[SerializeField] private AudioManager audioManager;
	[SerializeField] GameObject coinText;
	[SerializeField] GameObject marioLives;
	public GameObject playerDeathPrefab;
	Transform spawnPoint;

	float jumpHeight = 20f;
	float jumpMultiplier;

	private void Awake() {
		audioManager = FindObjectOfType<AudioManager>();
		marioLives = GameObject.Find("MarioLives");
		coinText = GameObject.Find("CoinCount");
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Void")) {
			Death();
		}

		if (collision.gameObject.CompareTag("Coin")) {
			coinText.GetComponent<Counter>().Add(1);
			audioManager.Play("Coin");
			Destroy(collision.gameObject);
		}

		if (collision.gameObject.CompareTag("Pipe") && collision.gameObject.GetComponent<Pipe>().warpPos != null)
		{
			Pipe pipe = collision.gameObject.GetComponent<Pipe>();
			pipe.getDirection(playerMovement);

			if (pipe.enterPipe) {
				playerMovement.enabled = false;
				if (playerMovement.isBig) animator.Play("Big_Mario_Idle"); else animator.Play("Small_Mario_Idle");
				playerMovement.animator.enabled = false;
				spriteRenderer.sortingOrder = -2;
				standCollider.enabled = false;
				crouchCollider.enabled = false;

				pipe.Warp(gameObject);

				rb.velocity = pipe.direction;
				rb.gravityScale = 0f;

				audioManager.Stop(pipe.theme);
				audioManager.Play("Pipe");
			}
		}

		if (collision.gameObject.CompareTag("Mushroom")) {
			StartCoroutine(PowerUp(1));
			audioManager.Play("Power Up");
			Destroy(collision.gameObject);
		}

		if (collision.gameObject.CompareTag("Fireflower")) {
			StartCoroutine(PowerUp(2));
			audioManager.Play("Power Up");
			Destroy(collision.gameObject);
		}

		if (collision.gameObject.CompareTag("1-Up")) {
			marioLives.GetComponent<Counter>().Add(1);
			audioManager.Play("1-Up");
			Destroy(collision.gameObject);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Enemy")) {
			if (collision.collider.bounds.max.y < transform.position.y &&
				transform.position.x > collision.collider.bounds.min.x - 0.5 &&
				transform.position.x < collision.collider.bounds.max.x + 0.5) {

				collision.gameObject.GetComponent<Collider2D>().enabled = false;
				rb.velocity = new Vector2(rb.velocity.x, jumpHeight * jumpMultiplier);
				audioManager.Play("Goomba Death");

			} else if (playerMovement.isBig && !playerMovement.isFire) {
				StartCoroutine(PowerUp(0));
				audioManager.Play("Pipe");
			} else if (playerMovement.isFire) {
				StartCoroutine(PowerUp(1));
				audioManager.Play("Pipe");
			} else {
				Death();
			}
		}
	}

	void Update() {
		if (Input.GetButtonDown("Reset")) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		if (Input.GetButton("Jump")) jumpMultiplier = 1.3f; else jumpMultiplier = 1f;
	}

	void Death() {
		spawnPoint = transform;
		Destroy(gameObject);
		Instantiate(playerDeathPrefab, spawnPoint.position, Quaternion.identity);

		audioManager.Stop("Main Theme");
		audioManager.Play("Player Death");
	}

	IEnumerator PowerUp(float result) {
		bool isBig;
		bool isFire;

		switch (result) {
			case 0:
				isFire = false;
				isBig = false;
				break;
			case 1:
				isFire = false;
				isBig = true;
				break;
			case 2:
				isBig = true;
				isFire = true;
				break;
			default:
				isFire = false;
				isBig = false;
				break;
		}

		bool wasBig = playerMovement.isBig;
		bool wasFire = playerMovement.isFire;

		playerMovement.isFire = isFire;
		playerMovement.isBig = isBig;
		for (int i = 0; i < 3; i++) {
			yield return new WaitForSeconds(0.09f);
			playerMovement.isFire = wasFire;
			playerMovement.isBig = wasBig;
			yield return new WaitForSeconds(0.09f);
			playerMovement.isFire = isFire;
			playerMovement.isBig = isBig;
		}
	}
	}
