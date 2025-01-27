using System.Collections;
using UnityEngine;

public class Pipe : MonoBehaviour {
    public Transform warpPos;

    public enum directions { Up, Down, Right, Left };
    public directions In;
    public directions Out;
    [HideInInspector]
    public Vector2 direction;

    AudioManager audioManager;
    Background background;

    [HideInInspector]
    public bool enterPipe;
    float warpSpeed = 3f;
    public string theme;
    public string world;

    void Awake() {
        audioManager = FindObjectOfType<AudioManager>();
        background = FindObjectOfType<Background>();
	}

	public void Warp(GameObject player) {
        getDirection(player.GetComponent<PlayerMovement>());
        StartCoroutine(Warp(player, warpPos));
    }

    public void getDirection(PlayerMovement playerMovement) {
        direction = directionIn(playerMovement);
    }

    IEnumerator Warp(GameObject player, Transform warpPos) {
        yield return new WaitForSeconds(0.46f);
        player.GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(1.04f);
        enterPipe = false;
        background.SwapBackgrounds(warpPos.GetComponent<Pipe>().world);
        player.transform.position = warpPos.position;
        warpPos.GetComponent<Collider2D>().enabled = false;
        player.GetComponent<SpriteRenderer>().enabled = true;
        player.GetComponent<Collider2D>().enabled = true;
        direction = warpPos.GetComponent<Pipe>().directionOut();
        player.GetComponent<Rigidbody2D>().velocity = direction;
        player.GetComponent<PlayerMovement>().horizontalMove = 0f;
        player.GetComponent<PlayerMovement>().lookUp = false;
        audioManager.Play("Pipe");

        yield return new WaitForSeconds(0.7f);
        warpPos.GetComponent<Collider2D>().enabled = true;
        player.GetComponent<Rigidbody2D>().gravityScale = 6f;
        player.GetComponent<SpriteRenderer>().sortingOrder = 100;
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<PlayerMovement>().crouch = false;
        player.GetComponent<Animator>().enabled = true;
        audioManager.Play(warpPos.GetComponent<Pipe>().theme);

	}

    Vector2 directionIn(PlayerMovement playerMovement) {
        switch (In) {
            case directions.Up:
                if (Input.GetButton("Up")) enterPipe = true;
                return Vector2.up * warpSpeed;
            case directions.Down:
                if (playerMovement.crouch) enterPipe = true;
                return Vector2.down * warpSpeed;
            case directions.Right:
                if (playerMovement.horizontalMove > 0.01f) enterPipe = true;
                return Vector2.right * warpSpeed;
            case directions.Left:
                if (playerMovement.horizontalMove < -0.01f) enterPipe = true;
                return Vector2.left * warpSpeed;
            default:
                return Vector2.zero;
        }
    }

    Vector2 directionOut() {
        switch (Out) {
            case directions.Up:
                return Vector2.up * warpSpeed;
            case directions.Down:
                return Vector2.down * warpSpeed;
            case directions.Right:
                return Vector2.right * warpSpeed;
            case directions.Left:
                return Vector2.left * warpSpeed;
            default:
                return Vector2.zero;
        }
    }
}
