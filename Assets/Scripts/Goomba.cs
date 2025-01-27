using UnityEngine;

public class Goomba : MonoBehaviour
{
	public Animator animator;
	[SerializeField] private Collider2D collide;
	[SerializeField] private Rigidbody2D rb;

	[SerializeField] Transform groundDetection;
	[SerializeField] LayerMask groundLayer;

	float speed = -100f;

	void Update() {

		if (Physics2D.Raycast(groundDetection.position, Vector2.right, 0.1f, groundLayer).collider) {
			speed *= -1f;
			transform.Rotate(0f, 180f, 0f);
		}

		if (!collide.enabled) {
			rb.bodyType = RigidbodyType2D.Static;
			animator.SetBool("isDead", true);
			Destroy(gameObject, 1f);
		}
	}

	void FixedUpdate() {
		if (collide.enabled) rb.velocity = new Vector2(speed * Time.fixedDeltaTime, rb.velocity.y);
	}
}
