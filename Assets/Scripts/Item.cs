using UnityEngine;

public class Item : MonoBehaviour
{
	public bool move;
	float speed = 200f;

	[SerializeField] Rigidbody2D rb;
	[SerializeField] Transform wallDetection;
	[SerializeField] LayerMask groundLayer;
	void Start() {
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		Collider2D[] playerColliders = player.GetComponents<Collider2D>();

		for (int i = 0; i < playerColliders.Length; i++) {
			Physics2D.IgnoreCollision(playerColliders[i], GetComponent<Collider2D>());
		}
	}
	void Update() {

		if (move && Physics2D.Raycast(wallDetection.position, Vector2.right, 0.1f, groundLayer).collider) {
			speed *= -1f;
			wallDetection.localPosition = new Vector2(-wallDetection.localPosition.x, wallDetection.localPosition.y);
		}

	}
	void FixedUpdate() {
		if (move) rb.velocity = new Vector2(speed * Time.fixedDeltaTime, rb.velocity.y);
	}
}
