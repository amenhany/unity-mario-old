using UnityEngine;

public class Parallax : MonoBehaviour {

    private float length, startPos;
    [SerializeField] private GameObject cam;
    [SerializeField] private float parallaxEffect;

	void Start() {
		startPos = transform.position.x;
		length = GetComponent<SpriteRenderer>().bounds.size.x;
	}

	void Update() {
		float temp = (cam.transform.position.x * (1 - parallaxEffect));
		float distance = (cam.transform.position.x * parallaxEffect);

		transform.position = new Vector2(startPos + distance, transform.position.y);

		if (temp > startPos + length) startPos += length;
		else if (temp < startPos - length) startPos -= length;
	}
}
