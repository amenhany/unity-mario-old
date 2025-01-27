using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryBlockHit : MonoBehaviour
{
	[SerializeField] GameObject coinText;
	[SerializeField] GameObject mushroomPrefab;
	[SerializeField] GameObject fireflowerPrefab;
	[SerializeField] GameObject oneUpPrefab;

	[SerializeField] AudioManager audioManager;
	[SerializeField] SpriteRenderer blockRenderer;
	[SerializeField] SpriteRenderer coinRenderer;
	[SerializeField] Animator animator;
	[SerializeField] Sprite used;

	Vector2 spawn;

	public enum item { Coins, Mushroom, Fireflower, Star, OneUp, Off };
	public item Item;

	[SerializeField] int totalCoins;

	private void Awake() {
		audioManager = FindObjectOfType<AudioManager>();
	}

	private void Start() {
		if (Item != item.Coins) coinRenderer.enabled = false;
		spawn = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 1);
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.collider.bounds.max.y < transform.position.y &&
			collision.collider.bounds.min.x < transform.position.x + 0.5f &&
			collision.collider.bounds.max.x > transform.position.x - 0.5f &&
			collision.collider.tag == "Player") {

			audioManager.Play("Hit Block");

			if (Item == item.Coins) {
				if (totalCoins > 0) {
					animator.Play("MysteryBlockCoin");
					audioManager.Play("Coin");
					coinText.GetComponent<Counter>().Add(1);
					totalCoins -= 1;
				}
				if (totalCoins == 0) {
					Disable();
				}
			}

			if (Item == item.Mushroom) {
				animator.Play("MysteryBlockMushroom");
				StartCoroutine(spawnItem(mushroomPrefab));
				Disable();
			}

			if (Item == item.Fireflower) {
				if (collision.collider.GetComponent<PlayerMovement>().isBig) {
					animator.Play("MysteryBlockFireflower");
					StartCoroutine(spawnItem(fireflowerPrefab));
					Disable();
				} else {
					animator.Play("MysteryBlockMushroom");
					StartCoroutine(spawnItem(mushroomPrefab));
					Disable();
				}
			}

			if (Item == item.OneUp) {
				animator.Play("MysteryBlockOneUp");
				StartCoroutine(spawnItem(oneUpPrefab));
				Disable();
			}
		}
	}

	IEnumerator spawnItem(GameObject prefab) {
		yield return new WaitForSeconds(0.04f);
		audioManager.Play("Power Up Appears");
		yield return new WaitForSeconds(0.71f);
		Instantiate(prefab, spawn, Quaternion.identity);
	}

	void Disable() {
		Item = item.Off;
		animator.SetBool("Empty", true);
		blockRenderer.sprite = used;
	}
}
